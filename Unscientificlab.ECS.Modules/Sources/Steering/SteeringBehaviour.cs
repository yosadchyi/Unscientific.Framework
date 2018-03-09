using Unscientificlab.ECS.Modules.Core;

namespace Unscientificlab.ECS.Modules.Steering
{
    public abstract class SteeringBehaviour
    {
        public SteeringVelocity Calculate (Entity<Simulation> owner, ref SteeringVelocity velocity)
        {
            return DoCalculate (owner, ref velocity);
        }

        public abstract SteeringVelocity DoCalculate (Entity<Simulation> owner, ref SteeringVelocity accumulatedSteering);

    }
}
