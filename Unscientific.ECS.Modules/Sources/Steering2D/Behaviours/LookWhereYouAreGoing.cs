using Unscientific.ECS.Modules.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class LookWhereYouAreGoing: ReachTargetOrientation
    {
        #region implemented abstract members of SteeringBehaviour

        public LookWhereYouAreGoing()
        {
        }

        public LookWhereYouAreGoing(Fix zeroVelocity) : base(zeroVelocity)
        {
        }

        public override SteeringVelocity DoCalculate (Entity<Game> owner, ref SteeringVelocity steering)
        {
            var linearVelocity = steering.Linear;

            if (linearVelocity.MagnitudeSqr <= ZeroVelocity * ZeroVelocity)
                return SteeringVelocity.Zero;

            var targetOrientation = FixMath.Atan2 (-linearVelocity.X, linearVelocity.Y);

            return DoReachOrientation(owner, targetOrientation);
        }

        #endregion
    }
}
