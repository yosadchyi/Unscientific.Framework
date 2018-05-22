namespace Unscientific.ECS.Features.BehaviourTree
{
    // ReSharper disable once UnusedTypeParameter
    public struct BehaviourTreeUpdatePeriod<TScope>
    {
        public readonly int PeriodInTicks;

        public BehaviourTreeUpdatePeriod(int periodInTicks)
        {
            PeriodInTicks = periodInTicks;
        }
    }
}