using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics
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
            foreach (var message in _messageBus.All<ComponentAdded<Game, Destroyed>>())
            {
                var entity = message.Entity;

                if (entity.Has<BoundingShape>())
                {
                    entity.Get<BoundingShape>().Shape.Return();
                    entity.Remove<BoundingShape>();
                }
            }
        }
    }
}