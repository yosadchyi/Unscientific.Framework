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

            foreach (var e in _features.SelectMany(m => m.ProducedMessages))
            {
                if (e.Delayed)
                {
                    messageBus.InitDelayed(e.MessageType, e.InitialCapacity, e.Aggregator);
                }
                else
                {
                    messageBus.Init(e.MessageType, e.InitialCapacity, e.Aggregator);
                }
            }

            return messageBus;
        }

        private Contexts CreateContexts()
        {
            var scopeToComponentTypes = _features.SelectMany(m => m.ProvidedComponents)
                .GroupBy(c => c.ScopeType)
                .ToDictionary(g => g.Key, g => g.Select(c => c.ComponentType).ToList());
            var contextsToCreate = new List<ContextInfo>();

            foreach (var e in _features.SelectMany(m => m.ProvidedContexts))
            {
                var scopeType = e.ScopeType;
                List<Type> components;

                if (!scopeToComponentTypes.TryGetValue(scopeType, out components))
                    components = new List<Type>();

                contextsToCreate.Add(new ContextInfo
                {
                    ScopeType = scopeType,
                    Components = components,
                    InitialCapacity = e.InitialCapacity,
                    MaxCapacity = e.MaxCapacity
                });
            }

            var contexts = new Contexts(contextsToCreate);
            return contexts;
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
