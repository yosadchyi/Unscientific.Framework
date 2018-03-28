using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable HeapView.ObjectAllocation.Evident

namespace Unscientific.ECS
{
    public class World
    {
        private readonly bool _fastMessageCleanup;
        public static World Instance { get; private set; }

        public MessageBus MessageBus { get; } = new MessageBus();
        public Contexts Contexts { get; } = new Contexts();
        public Systems Systems { get; }

        public class Builder
        {
            private readonly List<IModule> _modules = new List<IModule>();
            private bool _fastMessageCleanup;

            public Builder Uses(IModule module)
            {
                _modules.Add(module);
                return this;
            }

            public Builder WithFastMessageCleanup()
            {
                _fastMessageCleanup = true;
                return this;
            }

            public World Build()
            {
                return new World(_modules, _fastMessageCleanup);
            }
        }

        private World(List<IModule> modules, bool fastMessageCleanup)
        {
            _fastMessageCleanup = fastMessageCleanup;

            var sortedModules = TopologicalSort(modules);

            foreach (var module in sortedModules)
                module.Components().Register();

            foreach (var module in sortedModules)
                module.Contexts().Register();

            foreach (var module in sortedModules)
                module.Messages().Register(MessageBus);

            foreach (var module in sortedModules)
                module.Notifications().Register(Contexts, MessageBus);

            // add systems, in order matching module order in Uses clauses
            var builder = new Systems.Builder();

            foreach (var module in modules)
                builder.Add(module.Systems(Contexts, MessageBus));

            Systems = builder.ReverseCleanupSystemsOrder().Build();
            Instance = this;
        }

        private static List<IModule> TopologicalSort(List<IModule> modules)
        {
            var types = modules.Select(m => m.GetType()).ToList();
            var type2Module = modules.Select(m => m).ToDictionary(m => m.GetType(), m => m);
            var inDegree = modules.ToDictionary(m => m.GetType(), m => 0);
            var stack = new Stack<Type>();
            var queue = new Queue<Type>();

            foreach (var type in types)
            {
                var module = type2Module[type];

                foreach (var import in module.Usages().Imports)
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

                foreach (var import in module.Usages().Imports)
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

            if (_fastMessageCleanup)
            {
                MessageBus.FastCleanup();
            }
            else
            {
                MessageBus.Cleanup();
            }
        }

        public void Clear()
        {
            MessageBus.Clear();
            Contexts.Clear();
        }
    }
}