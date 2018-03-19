namespace Unscientific.ECS.Modules.Core
{
    public partial class Module<TTag>
    {
        public partial class Builder
        {
            public class ContextRegistrationsBuilder
            {
                private readonly Builder _builder;

                internal ContextRegistrationsBuilder(Builder builder)
                {
                    _builder = builder;
                }

                public ContextRegistrationsBuilder Add<TScope>(int initialCapacity = 128) where TScope : IScope
                {
                    _builder._contextRegistrations.Add<TScope>(initialCapacity);
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