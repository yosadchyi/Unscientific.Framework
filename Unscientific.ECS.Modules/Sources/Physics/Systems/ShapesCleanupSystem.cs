using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics.Systems
{
    public class ShapesCleanupSystem: ICleanupSystem
    {
        private readonly MessageBus _messageBus;

        public ShapesCleanupSystem(MessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void Cleanup()
        {
            foreach (var message in _messageBus.All<EntityDestroyed<Simulation>>())
            {
                var entity = message.Reference.Entity;

                if (entity.Has<BoundingShape>())
                {
                    entity.Get<BoundingShape>().Shape.Return();
                    entity.Remove<BoundingShape>();
                }
            }
        }
    }
}