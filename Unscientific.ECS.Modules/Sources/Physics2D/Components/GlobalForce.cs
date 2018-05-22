using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct GlobalForce
    {
        public readonly FixVec2 Value;

        public GlobalForce(FixVec2 value)
        {
            Value = value;
        }
    }
}