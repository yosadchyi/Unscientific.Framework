using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct Inertia
    {
        public readonly Fix Value;

        public Inertia(Fix value)
        {
            Value = value;
        }
    }
}