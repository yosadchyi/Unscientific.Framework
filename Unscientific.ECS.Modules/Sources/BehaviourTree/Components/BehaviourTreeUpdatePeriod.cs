namespace Unscientific.ECS.Modules.BehaviourTree
{
    public struct BehaviourTreeUpdatePeriod
    {
        public readonly int PeriodInTicks;

        public BehaviourTreeUpdatePeriod(int periodInTicks)
        {
            PeriodInTicks = periodInTicks;
        }
    }
}