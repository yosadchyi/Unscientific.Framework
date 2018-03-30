using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class ArriveBehaviour : SteeringBehaviour
    {
        private readonly Context<Game> _simulation;

        public ArriveBehaviour(Context<Game> simulation)
        {
            _simulation = simulation;
        }

        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            var target = FixVec2.Zero;

            if (!owner.TryGetTargetPosition(ref target))
                return SteeringVelocity.Zero;

            return Arrive(owner, target);
        }

        protected static SteeringVelocity Arrive(Entity<Game> owner, FixVec2 target)
        {
            var tolerance = owner.Get<ArrivalTolerance>();
            var position = owner.Get<Position>().Value;
            target.Sub(ref position);

            var distance = target.Magnitude;

            if (distance <= tolerance.Distance)
                return SteeringVelocity.Zero;

            var maxVelocity = owner.Get<MaxVelocity>().Value;
            var decelerationDistance = tolerance.DecelerationDistance;

            if (distance <= decelerationDistance)
                maxVelocity *= distance / decelerationDistance;

            var velocity = target / distance * maxVelocity;

            return new SteeringVelocity(FixVec2.ClampMagnitude(velocity, maxVelocity));
        }
    }
}