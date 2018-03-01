using System;

namespace Unscientificlab.ECS
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
            get { return MessageData<TMessage>.Data[_current]; }
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

    internal delegate void ClearDelegate();

    internal static class MessageData<TMessage>
    {
        internal static TMessage[] Data;
        // ReSharper disable once StaticMemberInGenericType
        internal static int Count;

        internal static void Init(int capacity)
        {
            Data = new TMessage[capacity];
        }

        internal static void Add(TMessage message)
        {
            if (Data == null)
                throw new MessageNotRegisteredException<TMessage>();
            if (Count == Data.Length)
                Array.Resize(ref Data, Data.Length * 2);
            Data[Count++] = message;
        }

        internal static void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                Data[i] = default(TMessage);
            }

            Count = 0;
        }
    }

    public class MessageRegistrations
    {
        private delegate void RegisterDelegate(MessageBus bus);

        private Delegate _delegate;

        public MessageRegistrations Add<TMessage>()
        {
            RegisterDelegate registerDelegate = (bus) => bus.Register<TMessage>();

            _delegate = _delegate == null ? registerDelegate : Delegate.Combine(_delegate, registerDelegate);

            return this;
        }

        public void Register(MessageBus bus)
        {
            _delegate?.DynamicInvoke(bus);
        }
    }
    
    public class MessageBus
    {
        public static MessageBus Instance { get; private set; }

        private event ClearDelegate OnClear = delegate { };

        public MessageBus()
        {
            Instance = this;
        }

        public MessageBus Register<TMessage>(int initialCapacity = 128)
        {
            MessageData<TMessage>.Init(initialCapacity);
            OnClear += MessageData<TMessage>.Clear;
            return this;
        }

        public void Send<TMessage>(TMessage message)
        {
            MessageData<TMessage>.Add(message);
        }

        public MessageEnumerable<TMessage> All<TMessage>()
        {
            if (MessageData<TMessage>.Data == null)
                throw new MessageNotRegisteredException<TMessage>();
            return new MessageEnumerable<TMessage>(MessageData<TMessage>.Count);
        }

        public void Clear<TMessage>()
        {
            MessageData<TMessage>.Clear();
        }
        
        /// <summary>
        /// Reset method must be called every frame.
        /// </summary>
        public void Clear()
        {
            OnClear();
        }
    }
}