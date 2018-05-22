using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct Velocity
    {
        public readonly FixVec2 Value;

        public Velocity(Fix x, Fix y)
        {
            Value = new FixVec2(x, y);
        }
        
        public Velocity(FixVec2 value)
        {
            Value = value;
        }
    }
}