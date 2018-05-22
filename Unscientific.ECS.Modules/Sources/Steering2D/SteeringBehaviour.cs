using Unscientific.ECS.Features.Core;

namespace Unscientific.ECS.Features.Steering2D
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
