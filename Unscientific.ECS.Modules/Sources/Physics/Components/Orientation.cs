using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public struct Orientation
    {
        public readonly Fix Value;

        public Orientation(Fix value)
        {
            Value = value;
        }
    }
}