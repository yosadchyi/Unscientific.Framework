using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Steering
{
    public abstract class SteeringBehaviour
    {
        public SteeringVelocity Calculate (Entity<Game> owner, ref SteeringVelocity velocity)
        {
            return DoCalculate (owner, ref velocity);
        }

        public abstract SteeringVelocity DoCalculate (Entity<Game> owner, ref SteeringVelocity accumulatedSteering);

    }
}
