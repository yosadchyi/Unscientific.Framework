using Unscientific.ECS.Features.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public class WeightedBehaviour: SteeringBehaviour
    {
        private readonly Fix _weight;
        private readonly SteeringBehaviour _behaviour;

        public WeightedBehaviour(Fix weight, SteeringBehaviour behaviour)
        {
            _weight = weight;
            _behaviour = behaviour;
        }

        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            return _behaviour.Calculate(owner, ref accumulatedSteering) * _weight;
        }
    }
}