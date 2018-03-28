using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public struct AngularDamping
    {
        public readonly Fix Value;

        public AngularDamping(Fix value)
        {
            Value = value;
        }
    }
}