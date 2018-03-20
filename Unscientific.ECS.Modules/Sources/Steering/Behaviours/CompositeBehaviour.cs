using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering
{
    public class BehaviourAndWeight
    {
        public SteeringBehaviour Behaviour;
        public Fix Weight;

        public BehaviourAndWeight(SteeringBehaviour behaviour, Fix weight)
        {
            Behaviour = behaviour;
            Weight = weight;
        }
    }

    public class CompositeBehaviour : SteeringBehaviour
    {
        private readonly BehaviourAndWeight[] _behaviours;

        public CompositeBehaviour(params BehaviourAndWeight[] behaviours)
        {
            _behaviours = behaviours;
        }

        public TBehaviour GetBehaviour<TBehaviour>() where TBehaviour : SteeringBehaviour
        {
            foreach (var behaviour in _behaviours)
            {
                var casted = behaviour.Behaviour as TBehaviour;

                if (casted != null)
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
                steering += behaviour.Behaviour.Calculate(owner, ref steering) * behaviour.Weight;
            }

            steering.Limit(maxVelocity, maxAngularVelocity);
            return steering;
        }
    }
}