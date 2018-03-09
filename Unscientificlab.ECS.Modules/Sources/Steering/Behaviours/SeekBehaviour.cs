using Framework.Unscientificlab;
using Unscientificlab.ECS.Modules.Core;
using Unscientificlab.ECS.Modules.Physics;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Steering
{
    public class SeekBehaviour : SteeringBehaviour
    {
        private readonly Context<Simulation> _simulation;

        public SeekBehaviour(Context<Simulation> simulation)
        {
            _simulation = simulation;
        }

        public override SteeringVelocity DoCalculate(Entity<Simulation> owner, ref SteeringVelocity accumulatedSteering)
        {
            var target = FixVec2.Zero;

            if (!owner.TryGetTargetPosition(_simulation, ref target))
                return SteeringVelocity.Zero;

            var maxVelocity = owner.Get<MaxVelocity>().Value;
            var position = owner.Get<Position>().Value;

            target.Sub(ref position);

            if (target.MagnitudeSqr == 0)
                return SteeringVelocity.Zero;

            target.Normalize();
            target.Scale(ref maxVelocity);

            return new SteeringVelocity
            {
                Linear = target
            };
        }
    }
}