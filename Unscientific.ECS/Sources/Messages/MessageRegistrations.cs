using System;
using System.Collections.Generic;

namespace Unscientific.ECS
{
    public class MessageRegistrations
    {
        private event Action<MessageBus> OnRegister = delegate {  };
        private static readonly HashSet<Type> RegisteredMessages = new HashSet<Type>();

        public MessageRegistrations Add<TMessage>(IMessageAggregator<TMessage> aggregator = null, int initialCapacity = 128)
        {
            return DoAdd<TMessage>(initialCapacity, capacity => MessageBus.Init(capacity, aggregator));
        }

        public MessageRegistrations AddDelayed<TMessage>(IMessageAggregator<TMessage> aggregator = null, int initialCapacity = 128)
        {
            return DoAdd<TMessage>(initialCapacity, capacity => MessageBus.InitDelayed(capacity, aggregator));
        }

        private MessageRegistrations DoAdd<TMessage>(int initialCapacity, Action<int> initializer)
        {
            OnRegister += bus =>
            {
                if (RegisteredMessages.Contains(typeof(TMessage)))
                    return;

                initializer(initialCapacity);
                RegisteredMessages.Add(typeof(TMessage));
            };
            return this;
        }

        public void Register(MessageBus bus)
        {
            OnRegister(bus);
        }
    }
}