using System;

namespace Unscientific.ECS.Modules.Core
{
    public static class ContextExtensions
    {
        public static Entity<TScope> Singleton<TScope>(this Context<TScope> self) where TScope : IScope
        {
            return self.FirstWith<SingletonTag>();
        }

        public static Entity<TScope> Singleton<TScope>(this Contexts self) where TScope : IScope
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