using System.Collections.Generic;

namespace Unscientificlab.ECS.Base
{
    public class Application
    {
        public MessageBus MessageBus { get; } = new MessageBus();
        public Contexts Contexts { get; } = new Contexts();
        public Systems Systems { get; }

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
                module.Messages().Register(MessageBus);

            foreach (var module in modules)
                module.Contexts().Register(referenceTrackerFactory);

            // add systems
            var builder = new Systems.Builder();

            foreach (var module in modules)
            {
                var systems = module.Systems(Contexts, MessageBus);

                builder.AddAll(systems);
            }

            Systems = builder.ReverseCleanupSystemsOrder().Build();
        }

        public void Setup()
        {
            Systems.Setup();
        }

        public void Update()
        {
            Systems.Update();
        }

        public void Cleanup()
        {
            Systems.Cleanup();
            MessageBus.Clear();
        }
    }
}