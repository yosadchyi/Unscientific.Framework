using Framework.Unscientificlab;
using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.ECS.Modules.Physics;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Steering
{
    public class LookAtTarget : ReachOrientationBehaviour
    {
        private readonly Context<Simulation> _simulation;
        
        #region implemented abstract members of SteeringBehaviour

        public LookAtTarget(Context<Simulation> simulation)
        {
            _simulation = simulation;
        }

        public override SteeringVelocity DoCalculate(Entity<Simulation> owner, ref SteeringVelocity steering)
        {
            var targetLocation = FixVec2.Zero;

            if (!owner.TryGetTargetPosition(_simulation, ref targetLocation))
                return SteeringVelocity.Zero;

            var direction = targetLocation - owner.Get<Position>().Value;

            if (direction.MagnitudeSqr <= ZeroVelocity * ZeroVelocity)
                return SteeringVelocity.Zero;

            var targetOrientation = FixMath.Atan2(direction.Y, -direction.X);

            return ReachOrientation(owner, targetOrientation);
        }

        #endregion
    }
}