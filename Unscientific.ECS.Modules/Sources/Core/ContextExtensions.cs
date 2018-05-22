namespace Unscientific.ECS.Modules.Core
{
    public static class ContextExtensions
    {
        public static Entity<TScope> Singleton<TScope>(this Context<TScope> self)
        {
            return self.FirstWith<SingletonTag>();
        }

        public static Entity<TScope> Singleton<TScope>(this Contexts self)
        {
            return Singleton(self.Get<TScope>());
        }

        public static Entity<Configuration> Configuration(this Contexts self)
        {
            return self.Singleton<Configuration>();
        }

        public static Entity<Singletons> Singleton(this Contexts self)
        {
            return self.Singleton<Singletons>();
        }
    }
}