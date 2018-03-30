using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public struct AlignTolerance
    {
        public readonly Fix Angle;
        public readonly Fix DecelerationAngle;

        public AlignTolerance(Fix angle, Fix decelerationAngle)
        {
            Angle = angle;
            DecelerationAngle = decelerationAngle;
        }
    }
}