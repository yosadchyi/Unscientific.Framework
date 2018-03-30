using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public struct Force
    {
        public readonly FixVec2 Value;

        public Force(FixVec2 value)
        {
            Value = value;
        }
    }
}