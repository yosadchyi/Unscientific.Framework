using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public struct TargetOrientation
    {
        public Fix Value;

        public TargetOrientation(Fix value)
        {
            Value = value;
        }
    }
}