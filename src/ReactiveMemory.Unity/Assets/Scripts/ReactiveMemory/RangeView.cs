using System;
using System.Collections;
using System.Collections.Generic;

namespace ReactiveMemory
{
    public readonly struct RangeView<T> : IEnumerable<T>, IReadOnlyList<T>, IList<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private readonly RangeView<T> _rangeView;
            private int _index;
            private readonly int _count;
            private T _current;

            public Enumerator(RangeView<T> rangeView)
            {
                this._rangeView = rangeView;
                _index = 0;
                _count = rangeView.Count;
                _current = default;
            }

            public bool MoveNext()
            {
                if (_index < _count)
                {
                    _current = _rangeView[_index];
                    _index++;
                    return true;
                }

                _current = default;
                _index = _count + 1;
                return false;
            }

            public void Reset()
            {
                _index = 0;
                _current = default;
            }

            public T Current => _current;

            object IEnumerator.Current => Current;

            public void Dispose() { }
        }

        public static RangeView<T> Empty => new RangeView<T>(null, -1, -1, false);

        private readonly T[] _orderedData;
        private readonly int _left;
        private readonly int _right;
        private readonly bool _ascendant;
        private readonly bool _hasValue;

        public int Count => !_hasValue ? 0 : _right - _left + 1;
        public T First => this[0];
        public T Last => this[Count - 1];

        public RangeView<T> Reverse => new RangeView<T>(_orderedData, _left, _right, !_ascendant);

        internal int FirstIndex => _ascendant ? _left : _right;
        internal int LastIndex => _ascendant ? _right : _left;

        bool ICollection<T>.IsReadOnly => true;

        public T this[int index]
        {
            get
            {
                if (!_hasValue) throw new ArgumentOutOfRangeException("view is empty");
                if (index < 0) throw new ArgumentOutOfRangeException("index < 0");
                if (Count <= index) throw new ArgumentOutOfRangeException("count <= index");

                return _ascendant ? _orderedData[_left + index] : _orderedData[_right - index];
            }
        }

        public RangeView(T[] orderedData, int left, int right, bool ascendant)
        {
            _hasValue = orderedData != null && orderedData.Length != 0 && left <= right; // same index is length = 1            this.orderedData = orderedData;
            _orderedData = orderedData;
            _left = left;
            _right = right;
            _ascendant = ascendant;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Any()
        {
            return Count != 0;
        }

        public int IndexOf(T item)
        {
            var i = 0;
            foreach (var v in this)
            {
                if (EqualityComparer<T>.Default.Equals(v, item))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public bool Contains(T item)
        {
            var count = Count;
            for (int i = 0; i < count; i++)
            {
                var v = this[i];
                if (EqualityComparer<T>.Default.Equals(v, item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var count = Count;
            Array.Copy(_orderedData, _left, array, arrayIndex, count);
            if (!_ascendant)
            {
                Array.Reverse(array, arrayIndex, count);
            }
        }

        T IList<T>.this[int index]
        {
            get => this[index];
            set => throw new NotImplementedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }
    }
}