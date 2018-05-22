using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public class SeekTarget : SteeringBehaviour
    {
        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            var target = FixVec2.Zero;

            if (!owner.TryGetTargetPosition(ref target))
                return SteeringVelocity.Zero;

            var maxVelocity = owner.Get<MaxVelocity>().Value;
            var position = owner.Get<Position>().Value;

            target.Sub(ref position);

            if (target.MagnitudeSqr == 0)
                return SteeringVelocity.Zero;

            target.Normalize();
            target.Scale(ref maxVelocity);

            return new SteeringVelocity
            {
                Linear = target
            };
        }
    }
}