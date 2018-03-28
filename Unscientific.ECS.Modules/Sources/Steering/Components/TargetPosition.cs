using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering
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