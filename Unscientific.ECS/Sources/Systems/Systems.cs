using System;

namespace Unscientific.ECS
{
    public class Systems : ISystems
    {
        private readonly Action<Contexts, MessageBus>[] _setupSystems;
        private readonly Action<Contexts, MessageBus>[] _updateSystems;
        private readonly Action<Contexts, MessageBus>[] _cleanupSystems;

        public Systems(Action<Contexts, MessageBus>[] setupSystems,
            Action<Contexts, MessageBus>[] updateSystems,
            Action<Contexts, MessageBus>[] cleanupSystems)
        {
            _setupSystems = setupSystems;
            _updateSystems = updateSystems;
            _cleanupSystems = cleanupSystems;
        }

        public void Setup(Contexts contexts, MessageBus messageBus)
        {
            foreach (var system in _setupSystems)
            {
                system(contexts, messageBus);
            }
        }
        
        public void Update(Contexts contexts, MessageBus messageBus)
        {
            foreach (var system in _updateSystems)
            {
                system(contexts, messageBus);
            }
        }

        public void Cleanup(Contexts contexts, MessageBus messageBus)
        {
            foreach (var system in _cleanupSystems)
            {
                system(contexts, messageBus);
            }
        }
    }
}