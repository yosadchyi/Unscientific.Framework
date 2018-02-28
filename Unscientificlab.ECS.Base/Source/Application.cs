using System.Collections.Generic;

namespace Unscientificlab.ECS.Base
{
    public class Application
    {
        private readonly MessageBus _messageBus = new MessageBus();
        private readonly Contexts _contexts = new Contexts();
        private readonly Systems _systems;

        public class Builder
        {
            private readonly List<IModule> _modules = new List<IModule>();
            private ReferenceTrackerFactory _referenceTrackerFactory;

            public Builder WithReferenceTrackerFactory(ReferenceTrackerFactory referenceTrackerFactory)
            {
                _referenceTrackerFactory = referenceTrackerFactory;
                return this;
            }

            public Builder Using(IModule module)
            {
                _modules.Add(module);
                return this;
            }

            public Application Build()
            {
                return new Application(_referenceTrackerFactory, _modules);
            }
        }

        private Application(ReferenceTrackerFactory referenceTrackerFactory, IReadOnlyCollection<IModule> modules)
        {
            foreach (var module in modules)
                module.Components().Register();

            foreach (var module in modules)
                module.Messages().Register(_messageBus);

            foreach (var module in modules)
                module.Contexts().Register(referenceTrackerFactory);

            // add systems
            var builder = new Systems.Builder();

            foreach (var module in modules)
            {
                var systems = module.Systems(_contexts, _messageBus);

                builder.AddAll(systems);
            }

            _systems = builder.ReverseCleanupSystemsOrder().Build();
        }

        public void Setup()
        {
            _systems.Setup();
        }

        public void Update()
        {
            _systems.Update();
        }

        public void Cleanup()
        {
            _systems.Cleanup();
        }
    }
}