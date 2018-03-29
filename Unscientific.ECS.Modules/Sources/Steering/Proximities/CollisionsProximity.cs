using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering
{
    public class CollisionsProximity: Proximity
    {
        private readonly Context<Game> _simulation;
        public Fix MaxDistance;

        public CollisionsProximity(Context<Game> simulation): this(-1)
        {
            _simulation = simulation;
        }

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