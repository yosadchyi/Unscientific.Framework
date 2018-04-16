namespace Unscientific.ECS.Modules.BehaviourTree
{
    // ReSharper disable once UnusedTypeParameter
    public struct BehaviourTreeUpdatePeriod<TScope> where TScope: IScope
    {
        public readonly int PeriodInTicks;

        public BehaviourTreeUpdatePeriod(int periodInTicks)
        {
            PeriodInTicks = periodInTicks;
        }
    }
}