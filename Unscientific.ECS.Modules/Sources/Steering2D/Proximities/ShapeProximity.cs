using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;
using Unscientific.ECS.Modules.Physics2D.Shapes;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class ShapeProximity: Proximity
    {
        private readonly Shape _shape;

        public ShapeProximity(Shape shape)
        {
            _shape = shape;
        }

        public override int FindNeighbors(Entity<Game> entity, Callback callback)
        {
            if (!entity.Has<Collisions>())
                return 0;

            var position = entity.Get<Position>().Value;
            var collisions = entity.Get<Collisions>().List;

            foreach (var collision in collisions)
            {
                if (collision.SelfShape != _shape) continue;

                var other = collision.Other;
                var diff = position - entity.Get<Position>().Value;
                var dist2 = diff.MagnitudeSqr;

                callback(other, dist2);
            }
            return collisions.Count;
        }
    }
}