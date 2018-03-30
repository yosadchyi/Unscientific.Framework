using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class LookAtTarget : ReachOrientationBehaviour
    {
        #region implemented abstract members of SteeringBehaviour

        public LookAtTarget()
        {
        }

        public LookAtTarget(Fix zeroVelocity) : base(zeroVelocity)
        {
        }

        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity steering)
        {
            var targetLocation = FixVec2.Zero;

            if (!owner.TryGetTargetPosition(ref targetLocation))
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