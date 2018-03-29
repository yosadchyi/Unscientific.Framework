using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public struct TargetPosition
    {
        public FixVec2 Value;

        public TargetPosition(FixVec2 value)
        {
            Value = value;
        }
    }
}