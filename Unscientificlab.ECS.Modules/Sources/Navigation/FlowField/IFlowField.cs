using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Navigation
{
    /// <summary>
    /// Flow field interface.
    /// </summary>
    public interface IFlowField
    {
        /// <summary>
        /// Lookups normalized flow vector for given position.
        /// </summary>
        /// <param name="position">Position to lookup flow vector for.</param>
        /// <returns>Normalized flow vector.</returns>
        FixVec2 LookupFlowVector(FixVec2 position);
    }
}