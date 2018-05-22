using Unscientific.ECS.Features.Core;

namespace Unscientific.ECS.Features.Physics2D
{
    public static class CollisionsCleanupSystem
    {
        public static void Cleanup(Contexts contexts)
        {
            var context = contexts.Get<Game>();

            foreach (var entity in context.AllWith<Collisions>())
            {
                entity.Get<Collisions>().List.Clear();
            }
        }
    }
}