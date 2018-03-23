namespace Unscientific.ECS.Modules.Core
{
    public delegate ISystem SystemFactory(Contexts contexts, MessageBus messageBus);

    public partial class Module<TTag>
    {
        public partial class Builder
        {
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

                public Builder End()
                {
                    return _builder;
                }
            }
        }
    }
}