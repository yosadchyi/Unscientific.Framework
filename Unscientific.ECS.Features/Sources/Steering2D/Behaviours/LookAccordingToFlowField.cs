﻿using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public class LookAccordingToFlowField: ReachTargetOrientation
    {
        #region implemented abstract members of SteeringBehaviour

        public LookAccordingToFlowField()
        {
        }

        public LookAccordingToFlowField(Fix zeroVelocity) : base(zeroVelocity)
        {
        }

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

            return DoReachOrientation(owner, targetOrientation);
        }

        #endregion
    }
}
