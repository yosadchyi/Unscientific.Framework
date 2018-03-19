namespace Unscientific.ECS.Modules.Core
{
    public static class ContextExtensions
    {
        public static Entity<TScope> Singleton<TScope>(this Context<TScope> self) where TScope : IScope
        {
            return self.First();
        }
        
        public static Entity<TScope> Singleton<TScope>(this Contexts self) where TScope : IScope
        {
            return self.Get<TScope>().Singleton();
        }
    }
}