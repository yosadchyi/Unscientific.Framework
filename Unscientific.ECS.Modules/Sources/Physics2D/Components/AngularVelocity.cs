using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public struct AngularVelocity
    {
        public readonly Fix Value;

        public AngularVelocity(Fix value)
        {
            Value = value;
        }
    }
}