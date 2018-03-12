using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering
{
    public class ReachOrientationBehaviour : SteeringBehaviour
    {
        public Fix ZeroVelocity = FixMath.Epsilon;

        public override SteeringVelocity DoCalculate(Entity<Simulation> owner, ref SteeringVelocity accumulatedSteering)
        {
            if (!owner.Has<TargetOrientation>())
                return SteeringVelocity.Zero;

            return ReachOrientation(owner, owner.Get<TargetOrientation>().Value);
        }

        protected SteeringVelocity ReachOrientation(Entity<Simulation> owner, Fix targetOrientation)
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