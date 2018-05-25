using Unscientific.ECS.DSL;

namespace Unscientific.ECS
{
    public class World
    {
        public static World Instance { get; private set; }

        public readonly Contexts Contexts;
        public readonly MessageBus MessageBus;
        private readonly bool _fastMessageCleanup;
        private readonly ISystems _systems;

        internal World(Contexts contexts, MessageBus messageBus, bool fastMessageCleanup, ISystems systems)
        {
            Instance = this;
            Contexts = contexts;
            MessageBus = messageBus;
            _fastMessageCleanup = fastMessageCleanup;
            _systems = systems;
        }

        public void Setup()
        {
            _systems.Setup(Contexts, MessageBus);
        }
        
        public void Update()
        {
            _systems.Update(Contexts, MessageBus);
        }
        
        public void Cleanup()
        {
            _systems.Cleanup(Contexts, MessageBus);

            if (_fastMessageCleanup)
            {
                MessageBus.FastCleanup();
            }
            else
            {
                MessageBus.Cleanup();
            }
        }

        // TODO proper shutdown instead of "hard" clear
        public void Clear()
        {
            Contexts.Clear();
            MessageBus.Clear();
        }
    }
}