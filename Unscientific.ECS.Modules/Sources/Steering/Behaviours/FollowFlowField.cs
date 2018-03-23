﻿using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;

namespace Unscientific.ECS.Modules.Steering
{
    public class FollowFlowField : SteeringBehaviour
    {
        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            var steering = new SteeringVelocity();

            if (!owner.Has<FlowField>())
                return steering;

            var flowField = owner.Get<FlowField>().Field;
            var flowVector = flowField.LookupFlowVector(owner.Get<Position>().Value);
            var maxVelocity = owner.Get<MaxVelocity>().Value;

            steering.Linear = flowVector * maxVelocity;
            return steering;
        }
    }
}