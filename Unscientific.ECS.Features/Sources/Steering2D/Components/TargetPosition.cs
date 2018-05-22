using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public struct TargetPosition
    {
        public readonly FixVec2 Value;

        public TargetPosition(FixVec2 value)
        {
            Value = value;
        }
    }
}