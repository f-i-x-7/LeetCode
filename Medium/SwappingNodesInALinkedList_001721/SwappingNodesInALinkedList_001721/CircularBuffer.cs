using System.Collections;

namespace SwappingNodesInALinkedList_001721
{
    /// <summary>
    /// Circular buffer.
    /// </summary>
    /// <remarks>
    /// This class is not thread-safe.<br/>
    /// When writing to a full buffer:<br/>
    /// <see cref="PushBack(T)"/> -> removes this[0] / <see cref="PeekFront()"/>;<br/>
    /// <see cref="PushFront(T)"/> -> removes this[<see cref="Size"/>-1] / <see cref="PeekBack()"/>().
    /// </remarks>
    public sealed class CircularBuffer<T> : IEnumerable<T>
    {
        private readonly T[] buffer;

        /// <summary>
        /// The _start. Index of the first element in buffer.
        /// </summary>
        private int start;

        /// <summary>
        /// The _end. Index after the last element in the buffer.
        /// </summary>
        private int end;

        /// <summary>
        /// The _size. Buffer size.
        /// </summary>
        private int size;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularBuffer{T}"/> class.
        /// </summary>
        /// <param name='capacity'>
        /// Buffer capacity. Must be positive.
        /// </param>
        public CircularBuffer(int capacity)
            : this(capacity, Array.Empty<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularBuffer{T}"/> class.
        /// </summary>
        /// <param name='capacity'>
        /// Buffer capacity. Must be positive.
        /// </param>
        /// <param name='items'>
        /// Items to fill buffer with. Items length must be less than or equal to capacity.
        /// </param>
        public CircularBuffer(int capacity, T[] items)
        {
            if (capacity < 1)
            {
                throw new ArgumentException(
                    "Circular buffer cannot have negative or zero capacity.", nameof(capacity));
            }
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Length > capacity)
            {
                throw new ArgumentException(
                    "Too many items to fit circular buffer", nameof(items));
            }

            buffer = new T[capacity];

            Array.Copy(items, buffer, items.Length);
            size = items.Length;

            start = 0;
            end = size == capacity ? 0 : size;
        }

        /// <summary>
        /// Maximum capacity of the buffer. Elements pushed into the buffer after
        /// maximum capacity is reached (<see cref="IsFull"/> = <see langword="true"/>), will remove an element.
        /// </summary>
        public int Capacity => buffer.Length;

        /// <summary>
        /// Boolean indicating if Circular is at full capacity.
        /// Adding more elements when the buffer is full will
        /// cause elements to be removed from the other end
        /// of the buffer.
        /// </summary>
        public bool IsFull => Size == Capacity;

        /// <summary>
        /// <see langword="true"/> if has no elements; otherwise, <see langword="false"/>.
        /// </summary>
        public bool IsEmpty => Size == 0;

        /// <summary>
        /// Current buffer size (the number of elements that the buffer has).
        /// </summary>
        public int Size => size;

        /// <summary>
        /// Element at the front of the buffer - this[0].
        /// </summary>
        /// <returns>The element at the front of the buffer.</returns>
        public T PeekFront()
        {
            ThrowIfEmpty();
            return buffer[start];
        }

        /// <summary>
        /// Element at the back of the buffer - this[<see cref="Size"/> - 1].
        /// </summary>
        /// <returns>The element at the back of the buffer.</returns>
        public T PeekBack()
        {
            ThrowIfEmpty();
            return buffer[(end != 0 ? end : Capacity) - 1];
        }

        /// <summary>
        /// Index access to elements in buffer.
        /// Index does not loop around like when adding elements,
        /// valid interval is [0; Size).
        /// </summary>
        /// <param name="index">Index of element to access.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when index is outside of [0; Size) interval.</exception>
        public T this[int index]
        {
            get
            {
                if (IsEmpty)
                {
                    ThrowIndexOutOfRangeException($"Cannot access index {index}. Buffer is empty.");
                }
                if (index >= size)
                {
                    ThrowIndexOutOfRangeException($"Cannot access index {index}. Buffer size is {size}.");
                }
                var actualIndex = InternalIndex(index);
                return buffer[actualIndex];
            }
            set
            {
                if (IsEmpty)
                {
                    ThrowIndexOutOfRangeException($"Cannot access index {index}. Buffer is empty.");
                }
                if (index >= size)
                {
                    ThrowIndexOutOfRangeException($"Cannot access index {index}. Buffer size is {size}.");
                }
                var actualIndex = InternalIndex(index);
                buffer[actualIndex] = value;
            }
        }

        /// <summary>
        /// Pushes a new element to the back of the buffer. <see cref="PeekBack()"/>/this[<see cref="Size"/>-1]
        /// will now return this element.
        /// 
        /// When the buffer is full, the element at <see cref="PeekFront()"/>/this[0] will be 
        /// popped to allow for this new element to fit.
        /// </summary>
        /// <param name="item">Item to push to the back of the buffer.</param>
        public void PushBack(T item)
        {
            if (IsFull)
            {
                buffer[end] = item;
                Increment(ref end);
                start = end;
            }
            else
            {
                buffer[end] = item;
                Increment(ref end);
                ++size;
            }
        }

        /// <summary>
        /// Pushes a new element to the front of the buffer. <see cref="PeekFront()"/>/this[0]
        /// will now return this element.
        /// 
        /// When the buffer is full, the element at <see cref="PeekBack()"/>/this[<see cref="Size"/>-1] will be 
        /// popped to allow for this new element to fit.
        /// </summary>
        /// <param name="item">Item to push to the front of the buffer</param>
        public void PushFront(T item)
        {
            if (IsFull)
            {
                Decrement(ref start);
                end = start;
                buffer[start] = item;
            }
            else
            {
                Decrement(ref start);
                buffer[start] = item;
                ++size;
            }
        }

        /// <summary>
        /// Removes the element at the back of the buffer. Decreasing the 
        /// Buffer size by 1.
        /// </summary>
        /// <returns>Removed element at the back of the buffer.</returns>
        public T PopBack()
        {
            ThrowIfEmpty();
            Decrement(ref end);
            var result = buffer[end];
            buffer[end] = default(T);
            --size;
            return result;
        }

        /// <summary>
        /// Removes the element at the front of the buffer. Decreasing the 
        /// Buffer size by 1.
        /// </summary>
        /// <returns>Removed element at the front of the buffer.</returns>
        public T PopFront()
        {
            ThrowIfEmpty();
            var result = buffer[start];
            buffer[start] = default(T);
            Increment(ref start);
            --size;
            return result;
        }

        #region IEnumerable<T> implementation

        /// <summary>
        /// Returns an enumerator that iterates through this buffer.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate this collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var firstSegment = ArraySegmentOne();
            var secondSegment = ArraySegmentTwo();
            for (var i = 0; i < firstSegment.Count; i++)
            {
                yield return firstSegment.Array[firstSegment.Offset + i];
            }
            for (var i = 0; i < secondSegment.Count; i++)
            {
                yield return secondSegment.Array[secondSegment.Offset + i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        private static void ThrowIndexOutOfRangeException(string message) => throw new IndexOutOfRangeException(message);

        private void ThrowIfEmpty()
        {
            if (IsEmpty)
            {
                Throw();
            }


            static void Throw() => throw new InvalidOperationException("Cannot take element from empty buffer.");
        }

        /// <summary>
        /// Increments the provided index variable by one, wrapping
        /// around if necessary.
        /// </summary>
        /// <param name="index"></param>
        private void Increment(ref int index)
        {
            if (++index == Capacity)
            {
                index = 0;
            }
        }

        /// <summary>
        /// Decrements the provided index variable by one, wrapping
        /// around if necessary.
        /// </summary>
        /// <param name="index"></param>
        private void Decrement(ref int index)
        {
            if (index == 0)
            {
                index = Capacity;
            }
            index--;
        }

        /// <summary>
        /// Converts the index in the argument to an index in <code>_buffer</code>
        /// </summary>
        /// <returns>
        /// The transformed index.
        /// </returns>
        /// <param name='index'>
        /// External index.
        /// </param>
        private int InternalIndex(int index)
        {
            return start + (index < (Capacity - start) ? index : index - Capacity);
        }

        private ArraySegment<T> ArraySegmentOne()
        {
            if (IsEmpty)
            {
                return new ArraySegment<T>(Array.Empty<T>());
            }
            if (start < end)
            {
                return new ArraySegment<T>(buffer, offset: start, count: end - start);
            }
            return new ArraySegment<T>(buffer, offset: start, count: buffer.Length - start);
        }

        private ArraySegment<T> ArraySegmentTwo()
        {
            if (IsEmpty)
            {
                return new ArraySegment<T>(Array.Empty<T>());
            }
            if (start < end)
            {
                return new ArraySegment<T>(buffer, offset: end, count: 0);
            }
            return new ArraySegment<T>(buffer, offset: 0, count: end);
        }
    }
}
