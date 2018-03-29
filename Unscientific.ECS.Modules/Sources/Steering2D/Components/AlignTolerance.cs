using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public struct AlignTolerance
    {
        public Fix Angle;
        public Fix DecelerationAngle;

        public AlignTolerance(Fix angle, Fix decelerationAngle)
        {
            Angle = angle;
            DecelerationAngle = decelerationAngle;
        }
    }
}