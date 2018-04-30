using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class CompositeBehaviour : SteeringBehaviour
    {
        private readonly SteeringBehaviour[] _behaviours;

        public CompositeBehaviour(params SteeringBehaviour[] behaviours)
        {
            _behaviours = behaviours;
        }

        public TBehaviour GetBehaviour<TBehaviour>() where TBehaviour : SteeringBehaviour
        {
            foreach (var behaviour in _behaviours)
            {
                if (behaviour is TBehaviour casted)
                    return casted;
            }
            return null;
        }

        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedVelocity)
        {
            var steering = SteeringVelocity.Zero;
            var maxVelocity = Fix.MaxValue;
            var maxAngularVelocity = Fix.MaxValue;

            if (owner.Has<MaxVelocity>())
                maxVelocity = owner.Get<MaxVelocity>().Value;
            if (owner.Has<MaxAngularVelocity>())
                maxAngularVelocity = owner.Get<MaxAngularVelocity>().Value;

            foreach (var behaviour in _behaviours)
            {
                steering += behaviour.Calculate(owner, ref steering);
            }

            steering.Limit(maxVelocity, maxAngularVelocity);
            return steering;
        }
    }
}