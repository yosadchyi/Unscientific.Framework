using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct Damping
    {
        public readonly Fix Value;

        public Damping(Fix value)
        {
            Value = value;
        }
    }
}