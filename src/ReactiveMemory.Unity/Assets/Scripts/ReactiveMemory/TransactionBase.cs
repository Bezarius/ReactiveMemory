using ReactiveMemory.Internal;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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


        protected static List<TElement> DiffCore<TElement, TKey>(TElement[] array, TElement[] addOrReplaceData,
            Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IChangesQueue<TElement> changesQueue)
        {
            var newList = new List<TElement>(array.Length);
            var replaceIndexes = new Dictionary<int, TElement>();
            foreach (var data in addOrReplaceData)
            {
                var index = BinarySearch.FindFirst(array, keySelector(data), keySelector, comparer);
                if (index != -1)
                {
                    replaceIndexes.Add(index, data);
                    changesQueue?.EnqueueUpdate(data, array[index]);
                }
                else
                {
                    newList.Add(data);
                    changesQueue?.EnqueueAdd(data);
                }
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (replaceIndexes.TryGetValue(i, out var data))
                {
                    newList.Add(data);
                }
                else
                {
                    newList.Add(array[i]);
                }
            }

            return newList;
        }

        protected static TElement[] DiffCore2<TElement, TKey>(
            TElement[] array, TElement[] addOrReplaceData,
            Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IChangesQueue<TElement> changesQueue)
        {
            // Sort the keys in the addOrReplaceData array
            Array.Sort(addOrReplaceData, (a, b) => comparer.Compare(keySelector(a), keySelector(b)));

            // Arrays to store indices for updates and additions
            List<int> updateIndices = new List<int>();
            List<int> addIndices = new List<int>();

            int arrayIndex = 0;
            int addOrReplaceIndex = 0;


            while (arrayIndex < array.Length && addOrReplaceIndex < addOrReplaceData.Length)
            {
                TKey arrayKey = keySelector(array[arrayIndex]);
                TKey addOrReplaceKey = keySelector(addOrReplaceData[addOrReplaceIndex]);

                int comparisonResult = comparer.Compare(arrayKey, addOrReplaceKey);

                if (comparisonResult < 0)
                {
                    arrayIndex++;
                }
                else if (comparisonResult == 0)
                {
                    updateIndices.Add(arrayIndex);
                    arrayIndex++;
                    addOrReplaceIndex++;
                }
                else
                {
                    addIndices.Add(addOrReplaceIndex);
                    addOrReplaceIndex++;
                }
            }

            while (addOrReplaceIndex < addOrReplaceData.Length)
            {
                addIndices.Add(addOrReplaceIndex);
                addOrReplaceIndex++;
            }

            // Create the newArray with the correct length
            int newArrayLength = array.Length + addIndices.Count;
            var newArray = new TElement[newArrayLength];

            int newArrayIndex = 0;
            arrayIndex = 0;
            addOrReplaceIndex = 0;

            // Perform the merge using the saved indices
            while (arrayIndex < array.Length || addOrReplaceIndex < addIndices.Count)
            {
                if (arrayIndex < array.Length &&
                    (addOrReplaceIndex >= addIndices.Count || arrayIndex != updateIndices[addIndices[addOrReplaceIndex]]))
                {
                    newArray[newArrayIndex++] = array[arrayIndex++];
                }
                else
                {
                    newArray[newArrayIndex++] = addOrReplaceData[addIndices[addOrReplaceIndex++]];
                    changesQueue?.EnqueueAdd(newArray[newArrayIndex - 1]);
                }
            }

            return newArray;
        }
    }
}