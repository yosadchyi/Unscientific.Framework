using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public struct Position
    {
        public readonly FixVec2 Value;

        public Position(Fix x, Fix y)
        {
            Value = new FixVec2(x, y);
        }
        
        public Position(FixVec2 value)
        {
            Value = value;
        }
    }
}