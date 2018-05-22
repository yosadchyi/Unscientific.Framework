using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
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
