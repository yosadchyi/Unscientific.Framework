namespace Unscientific.ECS.Features.Core
{
    public static class GlobalComponentNotificationsEntityExtensions
    {
        public static Entity<Singletons> AddGlobalComponentAddedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentAddedListener<TScope, TComponent> listener)
        {
            self.Get<ComponentAddedListeners<TScope, TComponent>>().Listeners.Add(listener);
            return self;
        }

        public static Entity<Singletons> RemoveGlobalComponentAddedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentAddedListener<TScope, TComponent> listener)
        {
            self.Get<ComponentAddedListeners<TScope, TComponent>>().Listeners.Remove(listener);
            return self;
        }

        public static Entity<Singletons> AddGlobalComponentRemovedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentRemovedListener<TScope, TComponent> listener)
        {
            self.Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners.Add(listener);
            return self;
        }

        public static Entity<Singletons> RemoveGlobalComponentRemovedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentRemovedListener<TScope, TComponent> listener)
        {
            self.Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners.Remove(listener);
            return self;
        }

        public static Entity<Singletons> AddGlobalComponentReplacedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentReplacedListener<TScope, TComponent> listener)
        {
            self.Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners.Add(listener);
            return self;
        }

        public static Entity<Singletons> RemoveGlobalComponentReplacedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentReplacedListener<TScope, TComponent> listener)
        {
            self.Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners.Remove(listener);
            return self;
        }

        public static Entity<Singletons> AddGlobalComponentListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentListener<TScope, TComponent> listener)
        {
            return self
                .AddGlobalComponentAddedListener(listener)
                .AddGlobalComponentRemovedListener(listener)
                .AddGlobalComponentReplacedListener(listener);
        }
        
        public static Entity<Singletons> RemoveGlobalComponentListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentListener<TScope, TComponent> listener)
        {
            return self
                .RemoveGlobalComponentAddedListener(listener)
                .RemoveGlobalComponentRemovedListener(listener)
                .RemoveGlobalComponentReplacedListener(listener);
        }

    }
}