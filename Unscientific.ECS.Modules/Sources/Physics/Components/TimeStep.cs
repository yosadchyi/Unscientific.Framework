using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public struct TimeStep
    {
        public readonly Fix Value;

        public TimeStep(Fix value)
        {
            Value = value;
        }
    }
}
