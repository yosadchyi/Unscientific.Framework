using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public class ArriveToTarget : SteeringBehaviour
    {
        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            var target = FixVec2.Zero;

            return owner.TryGetTargetPosition(ref target) ? Arrive(owner, target) : SteeringVelocity.Zero;
        }

        private static SteeringVelocity Arrive(Entity<Game> owner, FixVec2 target)
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