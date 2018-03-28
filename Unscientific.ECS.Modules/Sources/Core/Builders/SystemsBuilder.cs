using System.Diagnostics.CodeAnalysis;

namespace Unscientific.ECS.Modules.Core
{
    public delegate ISystem SystemFactory(Contexts contexts, MessageBus messageBus);
    public delegate ISystem SystemFactoryOnlyWithContexts(Contexts contexts);
    public delegate ISystem SystemFactoryOnlyWithMessages(MessageBus messageBus);

    // ReSharper disable once UnusedTypeParameter
    public partial class Module<TTag>
    {
        public partial class Builder
        {
            [SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
            [SuppressMessage("ReSharper", "HeapView.ClosureAllocation")]
            public class SystemsBuilder
            {
                private readonly Builder _builder;

                internal SystemsBuilder(Builder builder)
                {
                    _builder = builder;
                }

                public SystemsBuilder Add(SystemFactory factory)
                {
                    _builder._systemFactories.Add(factory);
                    return this;
                }

                public SystemsBuilder Add(SystemFactoryOnlyWithContexts factory)
                {
                    _builder._systemFactories.Add((contexts, messageBus) => factory(contexts));
                    return this;
                }

                public SystemsBuilder Add(SystemFactoryOnlyWithMessages factory)
                {
                    _builder._systemFactories.Add((contexts, messageBus) => factory(messageBus));
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