﻿using Unscientific.ECS.Features.Core.Components;

namespace Unscientific.ECS.Features.Core
{
    public static class EntityExtensions
    {
        public static Entity<Singletons> AddComponentAddedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentAddedListener<TScope, TComponent> listener)
        {
            if (!self.Has<ComponentAddedListeners<TScope, TComponent>>())
                self.Add(new ComponentAddedListeners<TScope, TComponent>());
            self.Get<ComponentAddedListeners<TScope, TComponent>>().Listeners.Add(listener);
            return self;
        }

        public static Entity<Singletons> RemoveComponentAddedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentAddedListener<TScope, TComponent> listener)
        {
            if (self.Has<ComponentAddedListeners<TScope, TComponent>>())
                self.Get<ComponentAddedListeners<TScope, TComponent>>().Listeners.Remove(listener);
            return self;
        }

        public static Entity<Singletons> AddComponentRemovedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentRemovedListener<TScope, TComponent> listener)
        {
            if (!self.Has<ComponentRemovedListeners<TScope, TComponent>>())
                self.Add(new ComponentRemovedListeners<TScope, TComponent>());
            self.Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners.Add(listener);
            return self;
        }

        public static Entity<Singletons> RemoveComponentRemovedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentRemovedListener<TScope, TComponent> listener)
        {
            if (self.Has<ComponentRemovedListeners<TScope, TComponent>>())
                self.Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners.Remove(listener);
            return self;
        }

        public static Entity<Singletons> AddComponentReplacedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentReplacedListener<TScope, TComponent> listener)
        {
            if (!self.Has<ComponentReplacedListeners<TScope, TComponent>>())
                self.Add(new ComponentReplacedListeners<TScope, TComponent>());
            self.Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners.Add(listener);
            return self;
        }

        public static Entity<Singletons> RemoveComponentReplacedListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentReplacedListener<TScope, TComponent> listener)
        {
            if (self.Has<ComponentReplacedListeners<TScope, TComponent>>())
                self.Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners.Remove(listener);
            return self;
        }

        public static Entity<Singletons> AddComponentListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentListener<TScope, TComponent> listener)
        {
            return self
                .AddComponentAddedListener(listener)
                .AddComponentRemovedListener(listener)
                .AddComponentReplacedListener(listener);
        }
        
        public static Entity<Singletons> RemoveComponentListener<TScope, TComponent>(
            this Entity<Singletons> self,
            IComponentListener<TScope, TComponent> listener)
        {
            return self
                .RemoveComponentAddedListener(listener)
                .RemoveComponentRemovedListener(listener)
                .RemoveComponentReplacedListener(listener);
        }

    }
}