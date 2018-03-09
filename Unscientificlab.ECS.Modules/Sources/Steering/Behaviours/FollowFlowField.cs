using Unscientificlab.ECS.Modules.Core;
using Unscientificlab.ECS.Modules.Physics;

namespace Unscientificlab.ECS.Modules.Steering
{
    public class FollowFlowField : SteeringBehaviour
    {
        public override SteeringVelocity DoCalculate(Entity<Simulation> owner, ref SteeringVelocity accumulatedSteering)
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