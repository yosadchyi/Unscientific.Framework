namespace Unscientific.ECS.Modules.Core
{
    /// <summary>
    /// Component which indicates that entity is destroyed and should not be touched. 
    /// </summary>
    public struct Destroyed
    {
    }

    public struct SingletonTag
    {
    }

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