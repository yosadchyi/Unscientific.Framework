using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct AngularDamping
    {
        public readonly Fix Value;

        public AngularDamping(Fix value)
        {
            Value = value;
        }
    }
}