using System;

namespace Unscientific.ECS.DSL
{
    public class MessagesBuilder: NestedBuilder<FeatureBuilder>
    {
        public class Configurer<TMessage>
        {
            internal int InitialCapacity = 128;
            internal bool Delayed;
            internal IMessageAggregator<TMessage> Aggregator;

            public Configurer<TMessage> SetInitialCapacity(int initialCapacity)
            {
                InitialCapacity = initialCapacity;
                return this;
            }
        
            public Configurer<TMessage> DelayUntilNextFrame()
            {
                Delayed = true;
                return this;
            }

            public Configurer<TMessage> Aggregate(IMessageAggregator<TMessage> aggregator)
            {
                Aggregator = aggregator;
                return this;
            }
        }
        
        private readonly Action<MessageElement> _consumer;

        internal MessagesBuilder(FeatureBuilder parent, Action<MessageElement> consumer) : base(parent)
        {
            _consumer = consumer;
        }

        public MessagesBuilder Add<TMessage>()
        {
            return Add<TMessage>(c => c);
        }

        public MessagesBuilder Add<TMessage>(Func<Configurer<TMessage>, Configurer<TMessage>> configure)
        {
            var configurer = new Configurer<TMessage>();
            configure(configurer);
            MessageBus.InstantiateTypesForMessage<TMessage>();
            _consumer(new MessageElement(typeof(TMessage), configurer.InitialCapacity, configurer.Aggregator, configurer.Delayed));
            return this;
        }
    }
}