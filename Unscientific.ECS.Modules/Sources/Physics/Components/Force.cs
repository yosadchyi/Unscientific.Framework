using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public struct Force
    {
        public readonly FixVec2 Value;

        public Force(Fix x, Fix y)
        {
            Value = new FixVec2(x, y);
        }

        public Force(FixVec2 value)
        {
            Value = value;
        }
    }
}