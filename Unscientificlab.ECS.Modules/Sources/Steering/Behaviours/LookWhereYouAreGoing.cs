using Unscientificlab.ECS.Modules.Core;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Steering
{
    public class LookWhereYouAreGoing: ReachOrientationBehaviour
    {
        #region implemented abstract members of SteeringBehaviour

        public override SteeringVelocity DoCalculate (Entity<Simulation> owner, ref SteeringVelocity steering)
        {
            var linearVelocity = steering.Linear;

            if (linearVelocity.MagnitudeSqr <= ZeroVelocity * ZeroVelocity)
                return SteeringVelocity.Zero;

            var targetOrientation = FixMath.Atan2 (-linearVelocity.X, linearVelocity.Y);

            return ReachOrientation(owner, targetOrientation);
        }

        #endregion
    }
}
