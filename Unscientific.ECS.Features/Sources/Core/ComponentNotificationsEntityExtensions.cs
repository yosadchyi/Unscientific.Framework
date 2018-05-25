using Unscientific.ECS.Features.Destroy;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Features.Core
{
    public static class ComponentNotificationsEntityExtensions
    {
        public static Entity<TScope> AddComponentAddedListener<TScope, TComponent>(
            this Entity<TScope> self,
            IComponentAddedListener<TScope, TComponent> listener)
        {
            if (!self.Has<ComponentAddedListeners<TScope, TComponent>>())
            {
                var list = ListPool<IComponentAddedListener<TScope, TComponent>>.Instance.Get();

                self.Add(new ComponentAddedListeners<TScope, TComponent>(list));
            }

            self.Get<ComponentAddedListeners<TScope, TComponent>>().Listeners.Add(listener);
            return self;
        }

        public static Entity<TScope> RemoveComponentAddedListener<TScope, TComponent>(
            this Entity<TScope> self,
            IComponentAddedListener<TScope, TComponent> listener)
        {
            self.Get<ComponentAddedListeners<TScope, TComponent>>().Listeners.Remove(listener);
            return self;
        }

        public static Entity<TScope> AddComponentRemovedListener<TScope, TComponent>(
            this Entity<TScope> self,
            IComponentRemovedListener<TScope, TComponent> listener)
        {
            if (!self.Has<ComponentRemovedListeners<TScope, TComponent>>())
            {
                var list = ListPool<IComponentRemovedListener<TScope, TComponent>>.Instance.Get();

                self.Add(new ComponentRemovedListeners<TScope, TComponent>(list));
            }

            self.Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners.Add(listener);
            return self;
        }

        public static Entity<TScope> RemoveComponentRemovedListener<TScope, TComponent>(
            this Entity<TScope> self,
            IComponentRemovedListener<TScope, TComponent> listener)
        {
            self.Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners.Remove(listener);
            return self;
        }

        public static Entity<TScope> AddComponentReplacedListener<TScope, TComponent>(
            this Entity<TScope> self,
            IComponentReplacedListener<TScope, TComponent> listener)
        {
            if (!self.Has<ComponentReplacedListeners<TScope, TComponent>>())
            {
                var list = ListPool<IComponentReplacedListener<TScope, TComponent>>.Instance.Get();

                self.Add(new ComponentReplacedListeners<TScope, TComponent>(list));
            }

            self.Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners.Add(listener);
            return self;
        }

        public static Entity<TScope> RemoveComponentReplacedListener<TScope, TComponent>(
            this Entity<TScope> self,
            IComponentReplacedListener<TScope, TComponent> listener)
        {
            self.Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners.Remove(listener);
            return self;
        }

        public static Entity<TScope> AddComponentListener<TScope, TComponent>(
            this Entity<TScope> self,
            IComponentListener<TScope, TComponent> listener)
        {
            return self
                .AddComponentAddedListener(listener)
                .AddComponentRemovedListener(listener)
                .AddComponentReplacedListener(listener);
        }
        
        public static Entity<TScope> RemoveComponentListener<TScope, TComponent>(
            this Entity<TScope> self,
            IComponentListener<TScope, TComponent> listener)
        {
            return self
                .RemoveComponentAddedListener(listener)
                .RemoveComponentRemovedListener(listener)
                .RemoveComponentReplacedListener(listener);
        }

    }
}