using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering
{
    public struct ArrivalTolerance
    {
        public Fix Distance;
        public Fix DecelerationDistance;

        public ArrivalTolerance(Fix distance, Fix decelerationDistance)
        {
            Distance = distance;
            DecelerationDistance = decelerationDistance;
        }
    }
}