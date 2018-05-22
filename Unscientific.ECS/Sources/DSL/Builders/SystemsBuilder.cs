using System;
using System.Collections.Generic;

namespace Unscientific.ECS.DSL
{
    public class SystemsBuilder: NestedBuilder<FeatureBuilder>
    {
        private readonly Action<SystemsElement> _consume;
        private readonly List<SystemElement> _setupSystems = new List<SystemElement>();
        private readonly List<SystemElement> _updateSystems = new List<SystemElement>();
        private readonly List<SystemElement> _cleanupSystems = new List<SystemElement>();

        internal SystemsBuilder(FeatureBuilder parent, Action<SystemsElement> consume) : base(parent)
        {
            _consume = consume;
        }

        public SystemsBuilder Setup(Action<Contexts, MessageBus> action)
        {
            _setupSystems.Add(new SystemElement(action));
            return this;
        }

        public SystemsBuilder Update(Action<Contexts, MessageBus> action)
        {
            _updateSystems.Add(new SystemElement(action));
            return this;
        }

        public SystemsBuilder Cleanup(Action<Contexts, MessageBus> action)
        {
            _cleanupSystems.Add(new SystemElement(action));
            return this;
        }

        public override FeatureBuilder End()
        {
            var systemsElement = new SystemsElement();
            
            systemsElement.Add(_setupSystems, _updateSystems, _cleanupSystems);
            _consume(systemsElement);
            return base.End();
        }
    }
}