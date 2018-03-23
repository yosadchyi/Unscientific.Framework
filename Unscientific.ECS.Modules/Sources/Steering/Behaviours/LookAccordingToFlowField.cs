using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering
{
    public class LookAccordingToFlowField: ReachOrientationBehaviour
    {
        #region implemented abstract members of SteeringBehaviour

        public override SteeringVelocity DoCalculate (Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            var steering = new SteeringVelocity();

            if (!owner.Has<FlowField>())
                return steering;

            var flowField = owner.Get<FlowField>().Field;
            var flowVector = flowField.LookupFlowVector(owner.Get<Position>().Value);

            if (!owner.Has<TargetOrientation>())
                return SteeringVelocity.Zero;

            if (flowVector.MagnitudeSqr <= ZeroVelocity * ZeroVelocity)
                return SteeringVelocity.Zero;

            var targetOrientation = FixMath.Atan2(-flowVector.X, flowVector.Y);

            return ReachOrientation(owner, targetOrientation);
        }

        #endregion
    }
}
