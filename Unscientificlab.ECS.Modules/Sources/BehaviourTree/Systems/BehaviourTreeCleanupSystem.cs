using Unscientificlab.ECS.Modules.Base;

namespace Unscientificlab.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeCleanupSystem: ICleanupSystem
    {
        private readonly MessageBus _messageBus;

        public BehaviourTreeCleanupSystem(MessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        
        public void Cleanup()
        {
            foreach (var message in _messageBus.All<EntityDestroyed<Simulation>>())
            {
                var entity = message.Entity;

                if (!entity.Has<BehaviourTreeData>())
                    continue;

                var data = entity.Get<BehaviourTreeData>().ExecutionData;

                data.Return();
                entity.Remove<BehaviourTreeData>();
            }
        }
    }
}