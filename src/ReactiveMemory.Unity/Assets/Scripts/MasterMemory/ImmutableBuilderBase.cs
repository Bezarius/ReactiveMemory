using MasterMemory.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MasterMemory
{
    public abstract class ImmutableBuilderBase
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

        protected static List<TElement> RemoveCore<TElement, TKey>(TElement[] array, TKey key,
            Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IChangesQueue<TElement> changesQueue)
        {
            var newList = new List<TElement>(array.Length);
            for (var i = 0; i < array.Length; i++)
            {
                if (comparer.Compare(keySelector(array[i]), key) == 0)
                {
                    changesQueue.EnqueueRemove(array[i]);
                    continue;
                }

                newList.Add(array[i]);
            }

            return newList;
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
                    changesQueue.EnqueueUpdate(addOrReplaceData, array[i]);
                }
                else
                {
                    newList.Add(array[i]);
                }
            }

            if (!isUpdated)
            {
                newList.Add(addOrReplaceData);
                changesQueue.EnqueueAdd(addOrReplaceData);
            }

            return newList;
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
                    changesQueue.EnqueueUpdate(data, array[index]);
                }
                else
                {
                    newList.Add(data);
                    changesQueue.EnqueueAdd(data);
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
    }
}