using System;

namespace Unscientific.ECS.DSL
{
    public class ComponentListBuilder<TScope>: NestedBuilder<FeatureBuilder>
    {
        private readonly Action<ComponentElement> _consume;

        internal ComponentListBuilder(FeatureBuilder parent, Action<ComponentElement> consume) : base(parent)
        {
            _consume = consume;
        }

        public ComponentListBuilder<TScope> Add<TComponent>()
        {
            _consume(new ComponentElement(typeof(TScope), Context<TScope>.ComponentData<TComponent>.Init));
            return this;
        }
    }
}