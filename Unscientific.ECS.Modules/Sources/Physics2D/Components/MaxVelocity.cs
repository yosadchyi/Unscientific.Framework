using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public struct MaxVelocity
    {
        public readonly Fix Value;

        public MaxVelocity(Fix value)
        {
            Value = value;
        }
    }
}