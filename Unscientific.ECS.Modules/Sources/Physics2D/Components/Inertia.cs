using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public struct Inertia
    {
        public readonly Fix Value;

        public Inertia(Fix value)
        {
            Value = value;
        }
    }
}