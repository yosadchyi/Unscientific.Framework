using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
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