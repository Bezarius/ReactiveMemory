using System;
using System.Collections;
using System.Collections.Generic;

namespace ReactiveMemory
{
    public class SparseSet<T> : IEnumerable
    {
        private T[] _dense;
        private int[] _sparse;
        private int _count;

        public SparseSet(int capacity = 10)
        {
            _dense = new T[capacity];
            _sparse = new int[capacity];
            _count = 0;
        }

        public int Count => _count;

        public void Add(int objectId, T item)
        {
            if (Contains(objectId))
                return;

            if (objectId + 1 >= _sparse.Length)
                Array.Resize(ref _sparse, objectId + 1);

            if (_dense.Length<= _count + 1)
                Array.Resize(ref _dense, _dense.Length * 2);

            _dense[_count] = item;
            _sparse[objectId] = ++_count;
        }

        public T this[int objectId]
        {
            get
            {
                if (Contains(objectId))
                    return _dense[_sparse[objectId] - 1];
                throw new ArgumentException($"Object with index {objectId} does not exist in the SparseSet.");
            }
        }


        public void Remove(int objectId)
        {
            if (!Contains(objectId))
                return;

            var itemIndex = _sparse[objectId] - 1;
            var lastItem = _dense[_count - 1];

            _dense[itemIndex] = lastItem;
            _sparse[objectId] = itemIndex;

            _dense[_count - 1] = default(T);
            _sparse[objectId] = 0;

            _count--;
        }

        public bool Contains(int objectId)
        {
            return 
                objectId < _sparse.Length &&
                _sparse[objectId] > -1 &&
                _sparse[objectId] < _dense.Length &&
                _sparse[objectId] - 1 > -1 &&
                _dense[_sparse[objectId] - 1] != null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
            {
                yield return _dense[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}