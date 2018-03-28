using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public struct MaxAngularVelocity
    {
        public readonly Fix Value;

        public MaxAngularVelocity(Fix value)
        {
            Value = value;
        }
    }
}