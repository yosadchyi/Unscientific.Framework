using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics2D
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