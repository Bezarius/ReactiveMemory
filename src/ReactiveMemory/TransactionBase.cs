using ReactiveMemory.Internal;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReactiveMemory
{
    public abstract class TransactionBase
    {
        protected static TElement[] CloneAndSortBy<TElement, TKey>(IList<TElement> elementData,
            Func<TElement, TKey> indexSelector, IComparer<TKey> comparer)
        {
            var array = new TElement[elementData.Count];
            var sortSource = new TKey[elementData.Count];
            for (var i = 0; i < elementData.Count; i++)
            {
                array[i] = elementData[i];
                sortSource[i] = indexSelector(elementData[i]);
            }

            Array.Sort(sortSource, array, 0, array.Length, comparer);
            return array;
        }

        protected static TElement[] RemoveCore<TElement, TKey>(TElement[] array, TKey key,
            Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IChangesQueue<TElement> changesQueue)
        {
            var index = BinarySearch.FindFirst(array, key, keySelector, comparer);
            if (index >= 0)
            {
                changesQueue?.EnqueueRemove(array[index]);
                var newArray = new TElement[array.Length - 1];
                Array.Copy(array, 0, newArray, 0, index);
                Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);
                return newArray;
            }

            return array; // Key not found, return the original array.
        }

        protected static TElement[] RemoveCore<TElement, TKey>(TElement[] array, TKey[] keys,
            Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IChangesQueue<TElement> changesQueue)
        {
            TElement[] dest = null;
            for (var i = 0; i < keys.Length; i++)
            {
                dest = RemoveCore(array, keys[i],keySelector, comparer, changesQueue);
            }
            return dest;
        }
        
        protected static TElement[] DiffCore<TElement, TKey>(TElement[] array, TElement addOrReplaceData,
            Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IChangesQueue<TElement> changesQueue,
            bool createNewArray)
        {
            TElement[] dest;
            if (createNewArray)
            {
                dest = new TElement[array.Length];
                Array.Copy(array, dest, dest.Length);
            }
            else
            {
                dest = array;
            }

            var insertionIndex = BinarySearch.FindFirstOrExpectedIndex(array, keySelector(addOrReplaceData), keySelector, comparer);

            if (insertionIndex >= 0)
            {
                // Element found, update the array in place and enqueue the update if allowed
                var old = dest[insertionIndex];
                dest[insertionIndex] = addOrReplaceData;

                changesQueue?.EnqueueUpdate(addOrReplaceData, old);
            }
            else
            {
                insertionIndex = ~insertionIndex; // Convert the binary search result to the actual insertion index
                var newArray = new TElement[array.Length + 1];
                if (insertionIndex == 0)
                {
                    // Insert at the beginning of the array
                    newArray[0] = addOrReplaceData;
                    Array.Copy(dest, 0, newArray, 1, dest.Length);
                }
                else if (insertionIndex == dest.Length)
                {
                    // Insert at the end of the array
                    Array.Copy(dest, 0, newArray, 0, dest.Length);
                    newArray[dest.Length] = addOrReplaceData;
                }
                else
                {
                    // Insert in the middle of the array
                    Array.Copy(dest, 0, newArray, 0, insertionIndex);
                    newArray[insertionIndex] = addOrReplaceData;
                    Array.Copy(dest, insertionIndex, newArray, insertionIndex + 1, dest.Length - insertionIndex);
                }
                dest = newArray;
                changesQueue?.EnqueueAdd(addOrReplaceData);
            }

            return dest;
        }


        protected static TElement[] DiffCore<TElement, TKey>(TElement[] array, TElement[] addOrReplaceData,
            Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IChangesQueue<TElement> changesQueue,
            bool createNewArray = true)
        {
            TElement[] dest = null;
            for (var i = 0; i < addOrReplaceData.Length; i++)
            {
                dest = DiffCore(array, addOrReplaceData[i], keySelector, comparer, changesQueue, createNewArray);
                // after first iteration we don't need create array anymore
                createNewArray = false;
            }

            return dest;
        }
    }
}