using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct Orientation
    {
        public readonly Fix Value;

        public Orientation(Fix value)
        {
            Value = value;
        }
    }
}