namespace Unscientific.ECS.Modules.Core
{
    /// <summary>
    /// Tick component, used in Singletons context, holds number of ticks.
    /// </summary>
    public struct Tick
    {
        public readonly int Value;

        public Tick(int value)
        {
            Value = value;
        }
    }
}