using System;
using System.Collections.Generic;
using System.Threading;

namespace MasterMemory
{
    public struct ExpandableArray<TElement>
    {
        public TElement[] Items;
        private int _capacity;

        private int _count;

        public int Count => Volatile.Read(ref _count);

        public TElement this[int index] => Items[index];

        public ExpandableArray(object dummy)
        {
            Items = Array.Empty<TElement>();
            _count = 0;
            _capacity = Items.Length;
        }

        public void Add(TElement item)
        {
            int nextCount = Volatile.Read(ref _count) + 1;
            if (_capacity < nextCount)
            {
                Array.Resize(ref Items, checked(_count * 2));
            }

            Items[_count] = item;
            Volatile.Write(ref _count, nextCount);
        }

        public void EnsureCapacity(int capacity)
        {
            if (_capacity < capacity)
            {
                Array.Resize(ref Items, capacity);
                _capacity = capacity;
            }
        }

        public void AddRange(IEnumerable<TElement> source)
        {
            int count = Volatile.Read(ref _count);
            if (source is ICollection<TElement> collection)
            {
                EnsureCapacity(count + collection.Count);

                collection.CopyTo(Items, count);
                Volatile.Write(ref _count, count + collection.Count);
                return;
            }

            foreach (var item in source)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            Volatile.Write(ref _count, 0);
        }
    }
}