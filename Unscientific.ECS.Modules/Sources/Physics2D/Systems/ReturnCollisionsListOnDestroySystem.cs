using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Features.Physics2D
{
    public class ReturnCollisionsListOnDestroySystem
    {
        public static void Cleanup(Contexts contexts)
        {
            var context = contexts.Get<Game>();

            foreach (var entity in context.AllWith<Destroyed, Collisions>())
            {
                ListPool<Collision>.Instance.Return(entity.Get<Collisions>().List);
                entity.Remove<Collisions>();
            }
        }
    }
}