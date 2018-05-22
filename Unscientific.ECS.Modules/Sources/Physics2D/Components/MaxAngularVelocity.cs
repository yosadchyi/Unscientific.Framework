using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct MaxAngularVelocity
    {
        public readonly Fix Value;

        public MaxAngularVelocity(Fix value)
        {
            Value = value;
        }
    }
}