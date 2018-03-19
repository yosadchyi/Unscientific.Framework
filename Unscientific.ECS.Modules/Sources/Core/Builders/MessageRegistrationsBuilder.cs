namespace Unscientific.ECS.Modules.Core
{
    public partial class Module<TTag>
    {
        public partial class Builder
        {
            public class MessageRegistrationsBuilder
            {
                private readonly Builder _builder;

                internal MessageRegistrationsBuilder(Builder builder)
                {
                    _builder = builder;
                }

                public MessageRegistrationsBuilder Add<TMessage>(
                    IMessageAggregator<TMessage> aggregator = null,
                    int initialCapacity = 128)
                {
                    _builder._messageRegistrations.Add(aggregator, initialCapacity);
                    return this;
                }

                public MessageRegistrationsBuilder Add<TMessage>(int initialCapacity)
                {
                    return Add<TMessage>(null, initialCapacity);
                }

                public MessageRegistrationsBuilder AddDelayed<TMessage>(
                    IMessageAggregator<TMessage> aggregator = null,
                    int initialCapacity = 128)
                {
                    _builder._messageRegistrations.AddDelayed(aggregator, initialCapacity);
                    return this;
                }

                public MessageRegistrationsBuilder AddDelayed<TMessage>(int initialCapacity)
                {
                    _builder._messageRegistrations.AddDelayed<TMessage>(null, initialCapacity);
                    return this;
                }

                public Builder End()
                {
                    return _builder;
                }
            }
        }
    }
}