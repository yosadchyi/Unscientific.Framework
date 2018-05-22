using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public class ReachTargetOrientation : SteeringBehaviour
    {
        protected readonly Fix ZeroVelocity;

        public ReachTargetOrientation(): this(FixMath.Epsilon)
        {
        }

        public ReachTargetOrientation(Fix zeroVelocity)
        {
            ZeroVelocity = zeroVelocity;
        }

        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            if (!owner.Has<TargetOrientation>())
                return SteeringVelocity.Zero;

            return DoReachOrientation(owner, owner.Get<TargetOrientation>().Value);
        }

        protected static SteeringVelocity DoReachOrientation(Entity<Game> owner, Fix targetOrientation)
        {
            var tolerance = owner.Get<AlignTolerance>();
            var steering = SteeringVelocity.Zero;
            var orientation = owner.Get<Orientation>().Value;
            var rotation = ArithmeticUtils.WrapAngleAroundZero(targetOrientation - orientation);
            var rotationSize = FixMath.Abs(rotation);

            if (rotationSize <= tolerance.Angle)
                return steering;

            var maxAngularVelocity = owner.Get<MaxAngularVelocity>().Value;
            var angular = maxAngularVelocity * rotation / rotationSize;
            var decelerationAngle = tolerance.DecelerationAngle;

            if (rotationSize <= decelerationAngle) angular *= rotationSize / decelerationAngle;

            steering.Angular = angular;
            
            return steering;
        }
    }
}