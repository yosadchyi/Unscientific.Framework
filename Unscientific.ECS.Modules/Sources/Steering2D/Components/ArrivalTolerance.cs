using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public struct ArrivalTolerance
    {
        public readonly Fix Distance;
        public readonly Fix DecelerationDistance;

        public ArrivalTolerance(Fix distance, Fix decelerationDistance)
        {
            Distance = distance;
            DecelerationDistance = decelerationDistance;
        }
    }
}