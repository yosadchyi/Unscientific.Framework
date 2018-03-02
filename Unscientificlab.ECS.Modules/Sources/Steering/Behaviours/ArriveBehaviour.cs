using Framework.Unscientificlab;
using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.ECS.Modules.Physics;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Steering
{
    public class ArriveBehaviour : SteeringBehaviour
    {
        private readonly Context<Simulation> _simulation;

        public ArriveBehaviour(Context<Simulation> simulation)
        {
            _simulation = simulation;
        }

        public override SteeringVelocity DoCalculate(Entity<Simulation> owner, ref SteeringVelocity accumulatedSteering)
        {
            var target = FixVec2.Zero;

            if (!owner.TryGetTargetPosition(_simulation, ref target))
                return SteeringVelocity.Zero;

            return Arrive(owner, target);
        }

        protected SteeringVelocity Arrive(Entity<Simulation> owner, FixVec2 target)
        {
            var tolerance = owner.Get<ArrivalTolerance>();
            var position = owner.Get<Position>().Value;
            target.Sub(ref position);

            var distance = target.Magnitude;

            if (distance <= tolerance.Distance)
                return SteeringVelocity.Zero;

            var maxVelocity = owner.Get<MaxVelocity>().Value;
            var decelerationDistance = tolerance.DecelerationDistance;

            if (distance <= decelerationDistance)
                maxVelocity *= distance / decelerationDistance;

            var velocity = target / distance * maxVelocity;

            return new SteeringVelocity(FixVec2.ClampMagnitude(velocity, maxVelocity));
        }
    }
}