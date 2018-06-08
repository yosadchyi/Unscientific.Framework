using System;
using System.Collections.Generic;
using System.Linq;

namespace Unscientific.ECS.DSL
{
    public class WorldBuilder
    {
        public class WorldConfigurer
        {
            internal bool FastMessageCleanup;

            public WorldConfigurer UseFastMessageCleanup()
            {
                FastMessageCleanup = true;
                return this;
            }
        }

        private readonly WorldConfigurer _worldConfigurer = new WorldConfigurer();
        private readonly List<FeatureElement> _features = new List<FeatureElement>();

        public WorldBuilder()
        {
        }

        public WorldBuilder(Func<WorldConfigurer, WorldConfigurer> configure)
        {
            configure(_worldConfigurer);
        }

        public FeatureBuilder AddFeature(string name)
        {
            return new FeatureBuilder(this, feature => _features.Add(feature));
        }

        public World Build()
        {
            var contexts = CreateContexts();
            var messageBus = CreateMessageBus();
            var systems = CreateSystems();

            return new World(contexts, messageBus, _worldConfigurer.FastMessageCleanup, systems);
        }

        private ISystems CreateSystems()
        {
            var systems = _features.Select(m => SystemsFromElement(m.Systems)).ToArray();
            var compositeSystems = new CompositeSystems(systems);
            return compositeSystems;
        }

        private MessageBus CreateMessageBus()
        {
            var messageBus = new MessageBus();

            
            foreach (var e in _features.SelectMany(m => m.MessageCtors))
            {
                e(messageBus);
            }

            return messageBus;
        }

        private Contexts CreateContexts()
        {
            var scopeToComponentTypes = _features.SelectMany(m => m.ProvidedComponents)
                .GroupBy(c => c.ScopeType)
                .ToDictionary(g => g.Key, g => g.Select(c => c.ComponentCtor).ToList());
            var contexts = new List<IContext>();

            foreach (var e in _features.SelectMany(m => m.ProvidedContexts))
            {
                var scopeType = e.ScopeType;
                List<Func<ComponentInfo>> componentCtors;

                if (!scopeToComponentTypes.TryGetValue(scopeType, out componentCtors))
                    componentCtors = new List<Func<ComponentInfo>>();

                var info = new ContextInfo
                {
                    ComponentCtors = componentCtors,
                    InitialCapacity = e.InitialCapacity,
                    MaxCapacity = e.MaxCapacity
                };

                contexts.Add(e.ContextCtor(info));
            }

            return new Contexts(contexts);
        }

        private static Systems SystemsFromElement(SystemsElement element)
        {
            var setupSystems = element.SetupSystems.Select(e => e.Action).ToArray();
            var updateSystems = element.UpdateSystems.Select(e => e.Action).ToArray();
            var cleanupSystems = element.CleanupSystems.Select(e => e.Action).ToArray();

            return new Systems(setupSystems, updateSystems, cleanupSystems);
        }
    }
}
