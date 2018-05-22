using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Navigation2D
{
    /// <summary>
    /// Flow field which directs towards given destination point.
    /// </summary>
    public class ToDestinationFlowField: IFlowField
    {
        /// <summary>
        /// Destination to move to.
        /// </summary>
        private readonly FixVec2 _destination;

        /// <summary>
        /// Constructs new flow field.
        /// </summary>
        /// <param name="destination">Destination to move to.</param>
        public ToDestinationFlowField(FixVec2 destination)
        {
            _destination = destination;
        }

        /// <summary>
        /// Resolve flow vector pointing towards destination for given position.
        /// </summary>
        /// <param name="position">Position to resolve flow vector.</param>
        /// <returns>Flow vector.</returns>
        public FixVec2 LookupFlowVector(FixVec2 position)
        {
            var vector = _destination;
            vector.Sub(ref position);
            vector.Normalize();
            return vector;
        }
    }
}
