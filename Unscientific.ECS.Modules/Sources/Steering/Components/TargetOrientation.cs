using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering
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