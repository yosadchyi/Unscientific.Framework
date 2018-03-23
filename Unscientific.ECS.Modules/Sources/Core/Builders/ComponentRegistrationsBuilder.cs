namespace Unscientific.ECS.Modules.Core
{
    public partial class Module<TTag>
    {
        public partial class Builder
        {
            public class ComponentRegistrationsBuilder<TScope> where TScope : IScope
            {
                private readonly Builder _builder;

                public ComponentRegistrationsBuilder(Builder builder)
                {
                    _builder = builder;
                }

                public ComponentRegistrationsBuilder<TScope> Add<TComponent>()
                {
                    _builder._componentRegistrations
                        .For<TScope>()
                        .Add<TComponent>()
                        .End();
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