using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Features.Physics2D
{
    public static class ReturnShapesOnDestroySystem
    {
        public static void Cleanup(Contexts contexts)
        {
            var context = contexts.Get<Game>();

            foreach (var entity in context.AllWith<Destroyed, BoundingShapes>())
            {
                var list = entity.Get<BoundingShapes>().Shapes;

                list.Clear();
                list.ReturnToPool();
            }
        }
    }
}