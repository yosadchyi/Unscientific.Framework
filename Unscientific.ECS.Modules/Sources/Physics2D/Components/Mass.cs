using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct Mass
    {
        public readonly Fix Value;

        public Mass(Fix value)
        {
            Value = value;
        }
    }
}