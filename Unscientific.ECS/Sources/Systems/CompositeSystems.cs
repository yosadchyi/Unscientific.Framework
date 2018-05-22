using System.Linq;

namespace Unscientific.ECS
{
    public class CompositeSystems: ISystems
    {
        private readonly Systems[] _systems;
        private readonly Systems[] _systemsForCleanup;

        public CompositeSystems(Systems[] systems)
        {
            _systems = systems;
            _systemsForCleanup = systems.Reverse().ToArray();
        }

        public void Setup(Contexts contexts, MessageBus messageBus)
        {
            foreach (var systems in _systems)
            {
                systems.Setup(contexts, messageBus);
            }
        }

        public void Update(Contexts contexts, MessageBus messageBus)
        {
            foreach (var systems in _systems)
            {
                systems.Update(contexts, messageBus);
            }
        }

        public void Cleanup(Contexts contexts, MessageBus messageBus)
        {
            foreach (var systems in _systemsForCleanup)
            {
                systems.Cleanup(contexts, messageBus);
            }
        }
    }
}