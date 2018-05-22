using System;

namespace Unscientific.ECS.DSL
{
    public class FeatureDependenciesBuilder: NestedBuilder<FeatureBuilder>
    {
        private readonly Action<DependencyElement> _consume;

        internal FeatureDependenciesBuilder(FeatureBuilder parent, Action<DependencyElement> consume) : base(parent)
        {
            _consume = consume;
        }

        public FeatureDependenciesBuilder Feature(string name)
        {
            return this;
        }
    }
}