using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public struct TargetOrientation
    {
        public readonly Fix Value;

        public TargetOrientation(Fix value)
        {
            Value = value;
        }
    }
}