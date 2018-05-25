using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.ECS.Features.Physics2D.Shapes;

namespace Unscientific.ECS.Features.Steering2D
{
    public class ShapeProximity: Proximity
    {
        private readonly string _tag;

        public ShapeProximity(string tag)
        {
            _tag = tag;
        }

        public override int FindNeighbors(Entity<Game> entity, Callback callback)
        {
            if (!entity.Has<Collisions>())
                return 0;

            var position = entity.Get<Position>().Value;
            var collisions = entity.Get<Collisions>().List;

            foreach (var collision in collisions)
            {
                if (collision.SelfShape.Tag != _tag) continue;

                var other = collision.Other;
                var diff = position - entity.Get<Position>().Value;
                var dist2 = diff.MagnitudeSqr;

                callback(other, dist2);
            }
            return collisions.Count;
        }
    }
}