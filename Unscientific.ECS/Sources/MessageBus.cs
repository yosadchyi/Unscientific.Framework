using System;
using System.Collections.Generic;

namespace Unscientific.ECS
{
    public struct MessageEnumerator<TMessage>
    {
        private readonly int _count;
        private int _current;

        public MessageEnumerator(int count)
        {
            _count = count;
            _current = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return ++_current < _count;
        }

        public void Reset()
        {
            _current = -1;
        }

        public TMessage Current
        {
            get { return MessageData<TMessage>.Queue[_current]; }
        }
    }

    public struct MessageEnumerable<TMessage>
    {
    
        private readonly int _count;

        public MessageEnumerable(int count)
        {
            _count = count;
        }

        public MessageEnumerator<TMessage> GetEnumerator()
        {
            return new MessageEnumerator<TMessage>(_count);
        }
    }

    internal class MessageQueue<TMessage>
    {
        public int Count
        {
            get { return _count; }
        }

        public TMessage this[int index]
        {
            get { return _data[index]; }
        }
            
        private TMessage[] _data;
        private int _count;

        public MessageQueue(int capacity)
        {
            _data = new TMessage[capacity];
        }

        public void Add(TMessage message)
        {
            if (_data == null)
                throw new MessageNotRegisteredException<TMessage>();
            if (_count == _data.Length)
                Array.Resize(ref _data, _data.Length * 2);
            _data[_count++] = message;
        }

        public void Reset()
        {
            for (var i = 0; i < _count; i++)
            {
                _data[i] = default(TMessage);
            }

            _count = 0;
        }

        public void Clear()
        {
            for (var i = 0; i < _count; i++)
            {
                _data[i] = default(TMessage);
            }

            _count = 0;
        }
    }

    internal static class MessageData<TMessage>
    {
        internal static MessageQueue<TMessage> Queue;
        private static MessageQueue<TMessage> _nextFrameQueue;

        internal static void Init(int capacity, int nextFrameCapacity)
        {
            Queue = new MessageQueue<TMessage>(capacity);
            _nextFrameQueue = nextFrameCapacity >= 0 ? new MessageQueue<TMessage>(nextFrameCapacity) : null;
        }
        
        public static bool IsInitialized()
        {
            return Queue != null;
        }

        internal static void Add(TMessage message)
        {
            Queue.Add(message);
        }

        public static void AddNextFrame(TMessage message)
        {
            _nextFrameQueue.Add(message);
        }

        internal static void Cleanup()
        {
            if (_nextFrameQueue != null)
            {
                // swap current and next frame queue
                var tmp = Queue;
                Queue = _nextFrameQueue;
                _nextFrameQueue = tmp;
                _nextFrameQueue.Clear();
            }
            else
            {
                Queue.Clear();
            }
        }

        internal static void Clear()
        {
            _nextFrameQueue?.Clear();
            Queue.Clear();
        }
    }

    public class MessageRegistrations
    {
        private event Action<MessageBus> OnRegister = delegate {  };
        private static readonly HashSet<Type> RegisteredMessages = new HashSet<Type>();

        public MessageRegistrations Add<TMessage>(bool hasNextFrameQueue = false, int initialCapacity = 128)
        {
            OnRegister += bus =>
            {
                if (RegisteredMessages.Contains(typeof(TMessage)))
                    return;

                MessageData<TMessage>.Init(initialCapacity, hasNextFrameQueue ? initialCapacity : -1);
                MessageBus.OnCleanup += MessageData<TMessage>.Cleanup;
                MessageBus.OnClear += MessageData<TMessage>.Clear;
                RegisteredMessages.Add(typeof(TMessage));
            };
            return this;
        }

        public void Register(MessageBus bus)
        {
            OnRegister(bus);
        }
    }
    
    public class MessageBus
    {
        public static MessageBus Instance { get; private set; }

        internal static event Action OnCleanup = delegate { };
        internal static event Action OnClear = delegate { };

        public MessageBus()
        {
            Instance = this;
        }

        public void Send<TMessage>(TMessage message)
        {
            MessageData<TMessage>.Add(message);
        }

        public void SendNextFrame<TMessage>(TMessage message)
        {
            MessageData<TMessage>.AddNextFrame(message);
        }

        public MessageEnumerable<TMessage> All<TMessage>()
        {
            if (!MessageData<TMessage>.IsInitialized())
                throw new MessageNotRegisteredException<TMessage>();
            return new MessageEnumerable<TMessage>(MessageData<TMessage>.Queue.Count);
        }

        /// <summary>
        /// Cleanup method must be called every frame.
        /// </summary>
        public void Cleanup()
        {
            OnCleanup();
        }

        public void Clear()
        {
            OnClear();
        }

        public void Clear<TMessage>()
        {
            MessageData<TMessage>.Clear();
        }
    }
}