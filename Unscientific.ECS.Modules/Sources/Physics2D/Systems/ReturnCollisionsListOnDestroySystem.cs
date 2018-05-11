using Unscientific.ECS.Modules.Core;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Modules.Physics2D
{
    public class ReturnCollisionsListOnDestroySystem : EntityCleanupSystem<Game, Destroyed, Collisions>
    {
        public ReturnCollisionsListOnDestroySystem(Contexts contexts) : base(contexts)
        {
        }

        protected override void Cleanup(Entity<Game> entity)
        {
            if (!entity.Has<Collisions>()) return;
            ListPool<Collision>.Instance.Return(entity.Get<Collisions>().List);
        }
    }
}