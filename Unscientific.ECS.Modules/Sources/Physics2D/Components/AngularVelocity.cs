using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct AngularVelocity
    {
        public readonly Fix Value;

        public AngularVelocity(Fix value)
        {
            Value = value;
        }
    }
}