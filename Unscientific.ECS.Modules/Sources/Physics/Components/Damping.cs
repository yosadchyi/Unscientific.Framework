using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public struct Damping
    {
        public readonly Fix Value;

        public Damping(Fix value)
        {
            Value = value;
        }
    }
}