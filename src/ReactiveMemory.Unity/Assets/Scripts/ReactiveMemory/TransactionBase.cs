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
            for (int i = 0; i < elementData.Count; i++)
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
                TElement[] newArray = new TElement[array.Length - 1];
                Array.Copy(array, 0, newArray, 0, index);
                Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);
                return newArray;
            }
            else
            {
                return array; // Key not found, return the original array.
            }
        }

        protected static List<TElement> RemoveCore<TElement, TKey>(TElement[] array, TKey[] keys,
            Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IChangesQueue<TElement> changesQueue)
        {
            var removeIndexes = new HashSet<int>();
            foreach (var key in keys)
            {
                var index = BinarySearch.FindFirst(array, key, keySelector, comparer);
                if (index != -1)
                {
                    changesQueue.EnqueueRemove(array[index]);
                    removeIndexes.Add(index);
                }
            }

            var newList = new List<TElement>(array.Length - removeIndexes.Count);
            for (int i = 0; i < array.Length; i++)
            {
                if (!removeIndexes.Contains(i))
                {
                    newList.Add(array[i]);
                }
            }

            return newList;
        }

        protected static List<TElement> DiffCore<TElement, TKey>(TElement[] array, TElement addOrReplaceData,
            Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IChangesQueue<TElement> changesQueue)
        {
            var newList = new List<TElement>(array.Length);
            bool isUpdated = false;
            for (var i = 0; i < array.Length; i++)
            {
                if (comparer.Compare(keySelector(array[i]), keySelector(addOrReplaceData)) == 0)
                {
                    newList.Add(addOrReplaceData);
                    isUpdated = true;
                    changesQueue?.EnqueueUpdate(addOrReplaceData, array[i]);
                }
                else
                {
                    newList.Add(array[i]);
                }
            }

            if (!isUpdated)
            {
                newList.Add(addOrReplaceData);
                changesQueue?.EnqueueAdd(addOrReplaceData);
            }

            return newList;
        }

        protected static TElement[] DiffCore<TElement, TKey>(TElement[] array, TElement addOrReplaceData,
                Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IChangesQueue<TElement> changesQueue,
                bool createNewArray = true)
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

            int insertionIndex = BinarySearch.FindFirst(array, keySelector(addOrReplaceData), keySelector, comparer);

            if (insertionIndex >= 0)
            {
                // Element found, update the array in place and enqueue the update if allowed
                dest[insertionIndex] = addOrReplaceData;

                if (changesQueue != null)
                {
                    changesQueue.EnqueueUpdate(addOrReplaceData, array[insertionIndex]);
                }
            }
            else
            {
                insertionIndex = ~insertionIndex; // Convert the binary search result to the actual insertion index

                var newArray = new TElement[array.Length + 1];
                Array.Copy(dest, 0, newArray, 0, insertionIndex);
                newArray[insertionIndex] = addOrReplaceData;
                Array.Copy(dest, insertionIndex, newArray, insertionIndex + 1, dest.Length - insertionIndex);
                dest = newArray;
                if (changesQueue != null)
                {
                    changesQueue.EnqueueAdd(addOrReplaceData);
                }
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
            }
            return dest;
        }
    }
}