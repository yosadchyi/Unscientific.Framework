namespace Unscientific.ECS.Features.Tick
{
    /// <summary>
    /// Tick component, used in Singletons context, holds number of ticks.
    /// </summary>
    public struct TickCounter
    {
        public readonly int Value;

        public TickCounter(int value)
        {
            Value = value;
        }
    }
}