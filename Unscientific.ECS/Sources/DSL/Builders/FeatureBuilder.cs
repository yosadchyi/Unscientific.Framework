using System;
using System.Collections.Generic;

namespace Unscientific.ECS.DSL
{
    public class FeatureBuilder: NestedBuilder<WorldBuilder>
    {
        private readonly Action<FeatureElement> _consume;
        private readonly List<DependencyElement> _imports = new List<DependencyElement>();
        private readonly List<ContextElement> _providedContexts = new List<ContextElement>();
        private readonly List<ComponentElement> _providedComponents = new List<ComponentElement>();
        private readonly List<Action<MessageBus>> _producedMessages = new List<Action<MessageBus>>();
        private readonly SystemsElement _systems = new SystemsElement();

        internal FeatureBuilder(WorldBuilder parent, Action<FeatureElement> consume) : base(parent)
        {
            _consume = consume;
        }

        public FeatureDependenciesBuilder DependsOn()
        {
            return new FeatureDependenciesBuilder(this, usedFeature => _imports.Add(usedFeature));
        }

        public ContextListBuilder Contexts()
        {
            return new ContextListBuilder(this, contextElement => _providedContexts.Add(contextElement));
        }

        public ComponentListBuilder<TScope> Components<TScope>()
        {
            return new ComponentListBuilder<TScope>(this, componentElement => _providedComponents.Add(componentElement));
        }

        public MessagesBuilder Messages()
        {
            return new MessagesBuilder(this, componentElement => _producedMessages.Add(componentElement));
        }

        public SystemsBuilder Systems()
        {
            return new SystemsBuilder(this, systemsElement => _systems.Add(systemsElement));
        }

        public override WorldBuilder End()
        {
            _consume(new FeatureElement(_imports, _providedContexts, _providedComponents, _producedMessages, _systems));
            return base.End();
        }
    }
}