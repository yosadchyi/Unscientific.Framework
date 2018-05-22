using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public class CollisionsProximity: Proximity
    {
        public readonly Fix MaxDistance;

        public CollisionsProximity(Fix maxDistance)
        {
            MaxDistance = maxDistance;
        }

        public override int FindNeighbors(Entity<Game> entity, Callback callback)
        {
            if (!entity.Has<Collisions>())
                return 0;

            var position = entity.Get<Position>().Value;
            var collisions = entity.Get<Collisions>().List;

            foreach (var collision in collisions)
            {
                var other = collision.Other;
                var diff = position - entity.Get<Position>().Value;
                var dist2 = diff.MagnitudeSqr;

                if (MaxDistance <= 0 || dist2 <= MaxDistance * MaxDistance)
                    callback(other, dist2);
            }
            return collisions.Count;
        }
    }
}