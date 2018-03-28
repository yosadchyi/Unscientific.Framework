using System;
using System.Collections.Generic;

namespace Unscientific.Util.Collections
{
    /// <summary>
    /// A generic Deque class. It can be thought of as
    /// a double-ended queue, hence Deque. This allows for
    /// an O(1) AddFront, AddBack, RemoveFront, RemoveBack.
    /// The Deque also has O(1) indexed lookup, as it is backed
    /// by a circular array.
    /// </summary>
    /// <typeparam name="T">
    /// The type of objects to store in the deque.
    /// </typeparam>
    public class Deque<T>
    {
        /// <summary>
        /// The default capacity of the deque.
        /// </summary>
        private const int DefaultCapacity = 16;

        /// <summary>
        /// The first element offset from the beginning of the data array.
        /// </summary>
        private int _startOffset;

        /// <summary>
        /// The circular array holding the items.
        /// </summary>
        private T[] _buffer;

        /// <summary>
        /// Creates a new instance of the Deque class with
        /// the default capacity.
        /// </summary>
        public Deque() : this(DefaultCapacity)
        {
        }

        /// <summary>
        /// Creates a new instance of the Deque class with
        /// the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the Deque.</param>
        public Deque(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(capacity), "capacity is less than 0.");
            }

            Capacity = capacity;
        }

        private int _capacityClosestPowerOfTwoMinusOne;

        /// <summary>
        /// Gets or sets the total number of elements
        /// the internal array can hold without resizing.
        /// </summary>
        public int Capacity
        {
            get { return _buffer.Length; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "value",
                        "Capacity is less than 0.");
                }
                if (value < Count)
                {
                    throw new InvalidOperationException(
                        "Capacity cannot be set to a value less than Count");
                }
                if (null != _buffer && value == _buffer.Length)
                {
                    return;
                }

                // Create a new array and copy the old values.
                var powOfTwo = ClosestPowerOfTwoGreaterThan(value);

                value = powOfTwo;

                var newBuffer = new T[value];
                CopyTo(newBuffer, 0);

                // Set up to use the new buffer.
                _buffer = newBuffer;
                _startOffset = 0;
                _capacityClosestPowerOfTwoMinusOne = powOfTwo - 1;
            }
        }

        /// <summary>
        /// Gets whether or not the Deque is filled to capacity.
        /// </summary>
        public bool IsFull => Count == Capacity;

        /// <summary>
        /// Gets whether or not the Deque is empty.
        /// </summary>
        public bool IsEmpty => 0 == Count;

        private void EnsureCapacityFor(int numElements)
        {
            if (Count + numElements > Capacity)
            {
                Capacity = Count + numElements;
            }
        }

        private int ToBufferIndex(int index)
        {
            var bufferIndex = (index + _startOffset)
                              & _capacityClosestPowerOfTwoMinusOne;

            return bufferIndex;
        }

        // ReSharper disable once UnusedParameter.Local
        private void CheckIndexOutOfRange(int index)
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException(
                    "The supplied index is greater than the Count");
            }
            if (index < 0)
            {
                throw new IndexOutOfRangeException(
                    "The supplied index is less than 0");
            }
        }

        private static void CheckArgumentsOutOfRange(
            int length,
            int offset,
            int count)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "offset", "Invalid offset " + offset);
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "count", "Invalid count " + count);
            }

            if (length - offset < count)
            {
                throw new ArgumentException(
                    String.Format(
                        "Invalid offset ({0}) or count + ({1}) "
                        + "for source length {2}",
                        offset, count, length));
            }
        }

        private int ShiftStartOffset(int value)
        {
            _startOffset = ToBufferIndex(value);

            return _startOffset;
        }

        private int PreShiftStartOffset(int value)
        {
            var offset = _startOffset;
            ShiftStartOffset(value);
            return offset;
        }

        private int PostShiftStartOffset(int value)
        {
            return ShiftStartOffset(value);
        }

        #region ICollection

        /// <summary>
        /// Gets the number of elements contained in the Deque.
        /// </summary>
        public int Count { get; private set; }

        private void IncrementCount(int value)
        {
            Count = Count + value;
        }

        private void DecrementCount(int value)
        {
            Count = Math.Max(Count - value, 0);
        }

        private void ClearBuffer(int logicalIndex, int length)
        {
            var offset = ToBufferIndex(logicalIndex);

            if (offset + length > Capacity)
            {
                var len = Capacity - offset;
                Array.Clear(_buffer, offset, len);

                len = ToBufferIndex(logicalIndex + length);
                Array.Clear(_buffer, 0, len);
            }
            else
            {
                Array.Clear(_buffer, offset, length);
            }
        }

        /// <summary>
        /// Removes all items from the Deque.
        /// </summary>
        public void Clear()
        {
            if (Count > 0)
            {
                ClearBuffer(0, Count);
            }
            Count = 0;
            _startOffset = 0;
        }

        /// <summary>
        ///     Copies the elements of the Deque to a System.Array,
        ///     starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional System.Array that is the destination of
        ///     the elements copied from the Deque. The System.Array must
        ///     have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        ///     The zero-based index in array at which copying begins.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     array is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     arrayIndex is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     The number of elements in the source Deque is greater than
        ///     the available space from arrayIndex to the end of the
        ///     destination array.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (null == array)
            {
                throw new ArgumentNullException("array", "Array is null");
            }

            // Nothing to copy
            if (null == _buffer)
            {
                return;
            }

            CheckArgumentsOutOfRange(array.Length, arrayIndex, Count);

            if (0 != _startOffset
                && _startOffset + Count >= Capacity)
            {
                var lengthFromStart = Capacity - _startOffset;
                var lengthFromEnd = Count - lengthFromStart;

                Array.Copy(
                    _buffer, _startOffset, array, 0, lengthFromStart);

                Array.Copy(
                    _buffer, 0, array, lengthFromStart, lengthFromEnd);
            }
            else
            {
                Array.Copy(
                    _buffer, _startOffset, array, 0, Count);
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the element to get or set.
        /// </param>
        /// <returns>The element at the specified index</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is not a valid index in this deque
        /// </exception>
        public T this[int index]
        {
            get { return Get(index); }

            set { Set(index, value); }
        }

        /// <summary>
        /// Adds the provided item to the front of the Deque.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddFront(T item)
        {
            EnsureCapacityFor(1);
            _buffer[PostShiftStartOffset(-1)] = item;
            IncrementCount(1);
        }

        /// <summary>
        /// Adds the provided item to the back of the Deque.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddBack(T item)
        {
            EnsureCapacityFor(1);
            _buffer[ToBufferIndex(Count)] = item;
            IncrementCount(1);
        }

        /// <summary>
        /// Removes an item from the front of the Deque and returns it.
        /// </summary>
        /// <returns>The item at the front of the Deque.</returns>
        public T RemoveFront()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("The Deque is empty");
            }

            var result = _buffer[_startOffset];
            _buffer[PreShiftStartOffset(1)] = default(T);
            DecrementCount(1);
            return result;
        }

        /// <summary>
        /// Removes an item from the back of the Deque and returns it.
        /// </summary>
        /// <returns>The item in the back of the Deque.</returns>
        public T RemoveBack()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("The Deque is empty");
            }

            DecrementCount(1);
            var endIndex = ToBufferIndex(Count);
            var result = _buffer[endIndex];
            _buffer[endIndex] = default(T);

            return result;
        }

        /// <summary>
        /// Gets the value at the specified index of the Deque
        /// </summary>
        /// <param name="index">The index of the Deque.</param>
        /// <returns></returns>
        public T Get(int index)
        {
            CheckIndexOutOfRange(index);
            return _buffer[ToBufferIndex(index)];
        }

        /// <summary>
        /// Sets the value at the specified index of the
        /// Deque to the given item.
        /// </summary>
        /// <param name="index">The index of the deque to set the item.</param>
        /// <param name="item">The item to set at the specified index.</param>
        public void Set(int index, T item)
        {
            CheckIndexOutOfRange(index);
            _buffer[ToBufferIndex(index)] = item;
        }

        /// <summary>
        /// Determines the index of a specific item in the deque.
        /// </summary>
        /// <param name="item">The object to locate in the deque.</param>
        /// <returns>
        /// The index of the item if found in the deque; otherwise, -1.
        /// </returns>
        public int IndexOf(T item)
        {
            var index = 0;

            var equalityComparer = EqualityComparer<T>.Default;

            if (_startOffset + Count > Capacity)
            {
                for (var i = _startOffset; i < Capacity; i++)
                {
                    if (equalityComparer.Equals(_buffer[i], item))
                        return index;
                    index++;
                }

                var endIndex = ToBufferIndex(Count);
                for (var i = 0; i < endIndex; i++)
                {
                    if (equalityComparer.Equals(_buffer[i], item))
                        return index;
                    index++;
                }
            }
            else
            {
                var endIndex = _startOffset + Count;
                for (var i = _startOffset; i < endIndex; i++)
                {
                    if (equalityComparer.Equals(_buffer[i], item))
                        return index;
                    index++;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the Deque.
        /// </exception>
        public void RemoveAt(int index)
        {
            if (index == 0)
            {
                RemoveFront();
                return;
            }
            if (index == Count - 1)
            {
                RemoveBack();
                return;
            }

            RemoveRange(index, 1);
        }

        /// <summary>
        ///     Removes a range of elements from the view.
        /// </summary>
        /// <param name="index">
        ///     The index into the view at which the range begins.
        /// </param>
        /// <param name="count">
        ///     The number of elements in the range. This must be greater
        ///     than 0 and less than or equal to <see cref="Count"/>.
        /// </param>
        public void RemoveRange(int index, int count)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("The Deque is empty");
            }
            if (index > Count - count)
            {
                throw new IndexOutOfRangeException(
                    "The supplied index is greater than the Count");
            }

            // Clear out the underlying array
            ClearBuffer(index, count);

            if (index == 0)
            {
                // Removing from the beginning: shift the start offset
                ShiftStartOffset(count);
                Count -= count;
                return;
            }
            if (index == Count - count)
            {
                // Removing from the ending: trim the existing view
                Count -= count;
                return;
            }

            if ((index + (count / 2)) < Count / 2)
            {
                // Removing from first half of list

                // Move items up:
                //  [0, index) -> [count, count + index)
                var copyCount = index;
                var writeIndex = count;
                for (var j = 0; j < copyCount; j++)
                {
                    _buffer[ToBufferIndex(writeIndex + j)] = _buffer[ToBufferIndex(j)];
                }

                // Rotate to new view
                ShiftStartOffset(count);
            }
            else
            {
                // Removing from second half of list

                // Move items down:
                // [index + collectionCount, count) ->
                // [index, count - collectionCount)
                var copyCount = Count - count - index;
                var readIndex = index + count;
                for (var j = 0; j < copyCount; ++j)
                {
                    _buffer[ToBufferIndex(index + j)] = _buffer[ToBufferIndex(readIndex + j)];
                }
            }

            // Adjust valid count
            DecrementCount(count);
        }

        private static int ClosestPowerOfTwoGreaterThan(int x)
        {
            x--;
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (x + 1);
        }

    }
}