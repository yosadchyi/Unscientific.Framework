using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
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