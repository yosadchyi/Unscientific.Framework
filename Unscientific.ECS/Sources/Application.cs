using System;
using System.Collections.Generic;
using System.Linq;

namespace Unscientific.ECS
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

        private Application(ReferenceTrackerFactory referenceTrackerFactory, List<IModule> modules)
        {
            var sortedModules = TopologicalSort(modules);

            foreach (var module in sortedModules)
                module.Components().Register();

            foreach (var module in sortedModules)
                module.Contexts().Register(referenceTrackerFactory);

            foreach (var module in sortedModules)
                module.Messages().Register(MessageBus);

            // add systems
            var builder = new Systems.Builder();

            foreach (var module in sortedModules)
                builder.Add(module.Systems(Contexts, MessageBus));

            Systems = builder.ReverseCleanupSystemsOrder().Build();
        }

        private static List<IModule> TopologicalSort(List<IModule> modules)
        {
            var types = modules.Select(m => m.GetType()).ToList();
            var type2Module = modules.ToDictionary(m => m.GetType(), m => m);
            var inDegree = modules.ToDictionary(m => m.GetType(), m => 0);
            var stack = new Stack<Type>();
            var queue = new Queue<Type>();

            foreach (var type in types)
            {
                var module = type2Module[type];

                foreach (var import in module.Imports().Imports)
                {
                    if (!type2Module.ContainsKey(import))
                        throw new NoRequiredModuleException(import);

                    inDegree[import] = inDegree[import] + 1;
                }
            }
            
            foreach (var type in types)
            {
                if (inDegree[type] == 0)
                    queue.Enqueue(type);
            }

            var count = 0;

            while (queue.Count > 0)
            {
                var type = queue.Dequeue();
                var module = type2Module[type];

                stack.Push(type);
                
                foreach (var import in module.Imports().Imports)
                {
                    inDegree[import] = inDegree[import] - 1;
                    
                    if (inDegree[import] == 0)
                        queue.Enqueue(import);
                }

                count++;
            }

            if (count != modules.Count)
                throw new ModulesHaveCircularReferenceException();

            return stack.Select(t => type2Module[t]).ToList();
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
            MessageBus.Cleanup();
        }

        public void Clear()
        {
            MessageBus.Clear();
            Contexts.Clear();
        }
    }
}