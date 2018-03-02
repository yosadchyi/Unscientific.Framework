using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.ECS.Modules.Physics;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Steering
{
    public class CollisionsProximity: Proximity
    {
        private readonly Context<Simulation> _simulation;
        public Fix MaxDistance;

        public CollisionsProximity(Context<Simulation> simulation): this(-1)
        {
            _simulation = simulation;
        }

        public CollisionsProximity(Fix maxDistance)
        {
            MaxDistance = maxDistance;
        }

        public override int FindNeighbors(Entity<Simulation> entity, Callback callback)
        {
            if (!entity.Has<Collisions>())
                return 0;

            var position = entity.Get<Position>().Value;
            var collisions = entity.Get<Collisions>().List;

            foreach (var collision in collisions)
            {
                var other = _simulation.GetEntityById(collision.Other);
                var diff = position - entity.Get<Position>().Value;
                var dist2 = diff.MagnitudeSqr;

                if (MaxDistance <= 0 || dist2 <= MaxDistance * MaxDistance)
                    callback(other, dist2);
            }
            return collisions.Count;
        }
    }
}