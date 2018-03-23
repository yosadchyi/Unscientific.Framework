namespace Unscientific.ECS.Modules.Core
{
    /// <summary>
    /// Game scope, used for deterministic simulation
    /// </summary>
    public abstract class Game : IScope
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