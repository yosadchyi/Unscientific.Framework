using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Navigation
{
    /// <summary>
    /// Flow field which returns vector lying on circle with radius equivalent to distance from center to supplied position.
    /// </summary>
    public class CircularFlowField: IFlowField
    {
        private static readonly FixTrans2 Rotation = FixTrans2.MakeRotation(FixMath.Pi / 100);

        /// <summary>
        /// Returns vector lying on circle with radius equivalent to distance from center to supplied position.
        /// </summary>
        /// <param name="position">Position for which flow vector will be resolved.</param>
        /// <returns>Flow vector.</returns>
        public FixVec2 LookupFlowVector(FixVec2 position)
        {
            var next = Rotation * position;

            next.Sub(ref position);
            next.Normalize();
            return next;
        }
    }
}