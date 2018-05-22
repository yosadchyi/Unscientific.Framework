using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Navigation2D
{
    /// <summary>
    /// Flow field with fixed movement direction.
    /// </summary>
    public class MoveInDirectionFlowField: IFlowField
    {
        /// <summary>
        /// Direction of movement.
        /// </summary>
        private readonly FixVec2 _direction;

        /// <summary>
        /// Constructs new flow field with given direction.
        /// </summary>
        /// <param name="direction">Direction to move, should be normalized.</param>
        public MoveInDirectionFlowField(FixVec2 direction)
        {
            _direction = direction;
        }

        /// <summary>
        /// Resolves flow vector for given position.
        /// </summary>
        /// <param name="position">Position for which flow vector will be resolved.</param>
        /// <returns>Resolved flow vector.</returns>
        public FixVec2 LookupFlowVector(FixVec2 position)
        {
            return _direction;
        }
    }
}