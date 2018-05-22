namespace Unscientific.ECS.Features.Steering2D
{
    public struct SteeringUpdatePeriod
    {
        public readonly int PeriodInTicks;

        public SteeringUpdatePeriod(int periodInTicks)
        {
            PeriodInTicks = periodInTicks;
        }
    }
}