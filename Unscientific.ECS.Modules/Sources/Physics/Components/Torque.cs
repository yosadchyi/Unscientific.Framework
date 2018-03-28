using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public struct Torque
    {
        public readonly Fix Value;

        public Torque(Fix value)
        {
            Value = value;
        }
    }
}