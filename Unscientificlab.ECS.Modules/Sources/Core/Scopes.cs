namespace Unscientific.ECS.Modules.Core
{
    /// <summary>
    /// Simulation scope, used for deterministic simulation
    /// </summary>
    public class Simulation : IScope
    {
    }

    /// <summary>
    /// Configuration scope
    /// </summary>
    public class Configuration : IScope
    {
    }

    /// <summary>
    /// Singleton scope
    /// </summary>
    public class Singletons : IScope
    {
    }
}