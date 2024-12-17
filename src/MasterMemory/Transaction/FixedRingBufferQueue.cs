using System;
using System.Threading;

namespace MasterMemory
{
    public class FixedRingBufferQueue<T>
    {
        private readonly T[] _buffer;
        private readonly int _capacity;

        private int _head; // Write index
        private int _tail; // Read index

        public FixedRingBufferQueue(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            // Capacity must be a power of two for bitwise operations (optional).
            _capacity = capacity;
            _buffer = new T[_capacity];
            _head = 0;
            _tail = 0;
        }

        public bool Enqueue(T item)
        {
            int nextHead = (_head + 1) % _capacity;

            // Check if the buffer is full
            if (nextHead == _tail)
            {
                return false; // Queue is full
            }

            _buffer[_head] = item;
            // Memory barrier to ensure _buffer write happens before updating _head
            Volatile.Write(ref _head, nextHead);

            return true;
        }

        public bool TryDequeue(out T item)
        {
            if (_tail == _head)
            {
                item = default;
                return false; // Queue is empty
            }

            item = _buffer[_tail];
            // Memory barrier to ensure reading from _buffer before updating _tail
            Volatile.Write(ref _tail, (_tail + 1) % _capacity);

            return true;
        }

        public int Count
        {
            get
            {
                int currentHead = Volatile.Read(ref _head);
                int currentTail = Volatile.Read(ref _tail);
                if (currentHead >= currentTail)
                {
                    return currentHead - currentTail;
                }
                else
                {
                    return _capacity - (currentTail - currentHead);
                }
            }
        }

        public void Clear()
        {
            // Only the producer thread should call Clear
            _head = 0;
            _tail = 0;
        }
    }
}