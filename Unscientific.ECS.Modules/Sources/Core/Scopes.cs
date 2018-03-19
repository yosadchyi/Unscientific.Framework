namespace Unscientific.ECS.Modules.Core
{
    /// <summary>
    /// Simulation scope, used for deterministic simulation
    /// </summary>
    public abstract class Simulation : IScope
    {
    }

    /// <summary>
    /// Configuration scope
    /// </summary>
    public abstract class Configuration : IScope
    {
    }

    /// <summary>
    /// Singleton scope
    /// </summary>
    public abstract class Singletons : IScope
    {
    }
}