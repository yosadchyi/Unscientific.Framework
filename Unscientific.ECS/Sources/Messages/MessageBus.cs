using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Unscientific.ECS
{
    public struct MessageEnumerator<TMessage>
    {
        private readonly int _count;
        private int _current;

        public TMessage Current => MessageBus.Data<TMessage>.Queue[_current];

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

    internal interface IMessageQueue<TMessage>
    {
        int Count { get; }
        TMessage this[int index] { get; }

        void Add(ref TMessage message);
        void Cleanup();
        void Clear();
        void FastCleanup();
    }

    public delegate TKey MessageKeyExtractor<in TMessage, out TKey>(TMessage message);

    public interface IMessageAggregator<TMessage>
    {
        bool AddIfNotContains(ref TMessage message);
        void Clear();
        void FastClear();
    }

    internal class SimpleMessageQueue<TMessage>: IMessageQueue<TMessage>
    {
        private readonly IMessageAggregator<TMessage> _aggregator;

        public int Count { get; private set; }

        public TMessage this[int index] => _data[index];

        private TMessage[] _data;

        public static void InstantiateType()
        {
            // do nothing
        }

        public SimpleMessageQueue(int capacity, IMessageAggregator<TMessage> aggregator)
        {
            _aggregator = aggregator ?? new NullMessageAggregator<TMessage>();
            _data = new TMessage[capacity];
        }

        public void Add(ref TMessage message)
        {
            if (!_aggregator.AddIfNotContains(ref message))
                return;
            
            if (_data == null)
                throw new MessageNotRegisteredException<TMessage>();
            if (Count == _data.Length)
                Array.Resize(ref _data, _data.Length * 2);
            _data[Count++] = message;
        }

        public void Clear()
        {
            _aggregator.Clear();
            
            for (var i = 0; i < Count; i++)
            {
                _data[i] = default(TMessage);
            }

            Count = 0;
        }

        public void FastCleanup()
        {
            _aggregator.FastClear();
            Count = 0;
        }

        public void Cleanup()
        {
            Clear();
        }
    }

    internal class DelayedMessageQueue<TMessage> : IMessageQueue<TMessage>
    {
        private IMessageQueue<TMessage> _queue1;
        private IMessageQueue<TMessage> _queue2;

        public int Count => _queue1.Count;

        public TMessage this[int index] => _queue1[index];

        public static void InstantiateType()
        {
            // do nothing
        }

        public DelayedMessageQueue(int capacity, IMessageAggregator<TMessage> aggregator)
        {
            // same aggregator is shared between both queues
            _queue1 = new SimpleMessageQueue<TMessage>(capacity, aggregator);
            _queue2 = new SimpleMessageQueue<TMessage>(capacity, aggregator);
        }

        public void Add(ref TMessage message)
        {
            _queue2.Add(ref message);
        }

        public void Cleanup()
        {
            var tmp = _queue1;
            _queue1 = _queue2;
            _queue2 = tmp;
            // shared aggregator is cleared, so messages with same key can be sent next frame
            _queue2.Cleanup();
        }

        public void FastCleanup()
        {
            var tmp = _queue1;
            _queue1 = _queue2;
            _queue2 = tmp;
            // shared aggregator is cleared, so messages with same key can be sent next frame
            _queue2.FastCleanup();
        }

        public void Clear()
        {
            _queue1.Clear();
            _queue2.Clear();
        }
    }

    internal class NullMessageAggregator<TMessage>: IMessageAggregator<TMessage>
    {
        public static NullMessageAggregator<TMessage> Instance { get; } = new NullMessageAggregator<TMessage>();
        
        public bool AddIfNotContains(ref TMessage message)
        {
            return true;
        }

        public void Clear()
        {
        }

        public void FastClear()
        {
        }
    }

    public class KeyMessageAggregator<TMessage, TKey>: IMessageAggregator<TMessage>
    {
        private readonly MessageKeyExtractor<TMessage, TKey> _keyExtractor;
        private readonly HashSet<TKey> _presentKeys = new HashSet<TKey>();

        public KeyMessageAggregator(MessageKeyExtractor<TMessage, TKey> keyExtractor)
        {
            _keyExtractor = keyExtractor;
        }

        public bool AddIfNotContains(ref TMessage message)
        {
            var key = _keyExtractor(message);

            if (_presentKeys.Contains(key))
                return false;

            _presentKeys.Add(key);
            return true;
        }

        public void Clear()
        {
            _presentKeys.Clear();
        }

        public void FastClear()
        {
            _presentKeys.Clear();
        }
    }

    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    public class MessageBus
    {
        internal static class Data<TMessage>
        {
            internal static IMessageQueue<TMessage> Queue;

            internal static void SetQueue(IMessageQueue<TMessage> queue)
            {
                Queue = queue;
                OnCleanup += Queue.Cleanup;
                OnFastCleanup += Queue.FastCleanup;
                OnClear += Queue.Clear;
            }
            
            public static void InstantiateType()
            {
                // do nothing
            }
        }
        
        internal static event Action OnCleanup = delegate { };
        internal static event Action OnFastCleanup = delegate { };
        internal static event Action OnClear = delegate { };

        internal MessageBus()
        {
            OnCleanup = delegate { };
            OnFastCleanup = delegate { };
            OnCleanup = delegate { };
        }

        internal static void InstantiateTypesForMessage<TMessage>()
        {
            Data<TMessage>.InstantiateType();
            SimpleMessageQueue<TMessage>.InstantiateType();
        }

        internal static void InstantiateTypesForDelayedMessage<TMessage>()
        {
            Data<TMessage>.InstantiateType();
            DelayedMessageQueue<TMessage>.InstantiateType();
        }

        internal void Init(Type messageType, int capacity, object aggregator)
        {
            InitForQueueType(messageType, typeof(SimpleMessageQueue<>), capacity, aggregator);
        }

        internal void InitDelayed(Type messageType, int capacity, object aggregator)
        {
            InitForQueueType(messageType, typeof(DelayedMessageQueue<>), capacity, aggregator);
        }

        private static void InitForQueueType(Type messageType, Type queueGenericType, int capacity, object aggregator)
        {
            var queueType = queueGenericType.MakeGenericType(messageType);
            var queue = Activator.CreateInstance(queueType, capacity, aggregator);

            var dataGenericType = typeof(Data<>);
            var dataType = dataGenericType.MakeGenericType(messageType);
            dataType.GetMethod("SetQueue", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new[] {queue});
        }

        public void Send<TMessage>(TMessage message)
        {
            Data<TMessage>.Queue.Add(ref message);
        }

        public MessageEnumerable<TMessage> All<TMessage>()
        {
            if (Data<TMessage>.Queue == null)
                throw new MessageNotRegisteredException<TMessage>();
            return new MessageEnumerable<TMessage>(Data<TMessage>.Queue.Count);
        }

        public void Cleanup()
        {
            OnCleanup();
        }

        public void FastCleanup()
        {
            OnFastCleanup();
        }

        public void Clear()
        {
            OnClear();
        }

        public void Clear<TMessage>()
        {
            Data<TMessage>.Queue.Clear();
        }
    }
}