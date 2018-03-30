using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class ReachOrientationBehaviour : SteeringBehaviour
    {
        protected readonly Fix ZeroVelocity;

        public ReachOrientationBehaviour(): this(FixMath.Epsilon)
        {
        }

        public ReachOrientationBehaviour(Fix zeroVelocity)
        {
            ZeroVelocity = zeroVelocity;
        }

        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            if (!owner.Has<TargetOrientation>())
                return SteeringVelocity.Zero;

            return ReachOrientation(owner, owner.Get<TargetOrientation>().Value);
        }

        protected SteeringVelocity ReachOrientation(Entity<Game> owner, Fix targetOrientation)
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