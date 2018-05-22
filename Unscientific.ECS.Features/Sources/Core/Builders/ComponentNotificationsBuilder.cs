using System.Collections.Generic;
using Unscientific.ECS.DSL;
using Unscientific.ECS.Features.Core.Components;

namespace Unscientific.ECS.Features.Core
{
    public class ComponentNotificationsBuilder<TScope> : NestedBuilder<FeatureBuilder>
    {
        public ComponentNotificationsBuilder(FeatureBuilder parent) : base(parent)
        {
        }

        public ComponentNotificationsBuilder<TScope> AddAddedNotifications<TComponent>()
        {
            // @formatter:off
            Parent
                .Components<Singletons>()
                    .Add<ComponentAddedListeners<TScope, TComponent>>()
                .End()
                .Messages()
                    .Add<ComponentAdded<TScope, TComponent>>()
                .End()
                .Systems()
                    .Setup((contexts, bus) => {
                        var unused = new ComponentAddedMessageProducer<TScope, TComponent>(contexts, bus);
    
                        contexts.Singleton().Add(new ComponentAddedListeners<TScope, TComponent>(new List<IComponentAddedListener<TScope, TComponent>>()));
                    })
                    .Update((contexts, bus) => {
                        var listeners = contexts.Singleton().Get<ComponentAddedListeners<TScope, TComponent>>().Listeners;
                    
                        if (listeners.Count == 0)
                            return;
        
                        foreach (var message in bus.All<ComponentAdded<TScope, TComponent>>())
                        {
                            foreach (var listener in listeners)
                            {
                                listener.OnComponentAdded(message.Entity);
                            }
                        }
                    })
                .End();
            // @formatter:on
            return this;
        }

        public ComponentNotificationsBuilder<TScope> AddRemovedNotifications<TComponent>()
        {
            // @formatter:off
            Parent
                .Components<Singletons>()
                    .Add<ComponentRemovedListeners<TScope, TComponent>>()
                .End()
                .Messages()
                    .Add<ComponentRemoved<TScope, TComponent>>()
                .End()
                .Systems()
                    .Setup((contexts, bus) => {
                        var unused = new ComponentRemovedMessageProducer<TScope, TComponent>(contexts, bus);
    
                        contexts.Singleton().Add(new ComponentRemovedListeners<TScope, TComponent>(new List<IComponentRemovedListener<TScope, TComponent>>()));
                    })
                    .Update((contexts, bus) => {
                        var listeners = contexts.Singleton().Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners;
                    
                        if (listeners.Count == 0)
                            return;
        
                        foreach (var message in bus.All<ComponentRemoved<TScope, TComponent>>())
                        {
                            foreach (var listener in listeners)
                            {
                                listener.OnComponentRemoved(message.Entity, message.Component);
                            }
                        }
                    })
                .End();
            // @formatter:on
            return this;
        }

        public ComponentNotificationsBuilder<TScope> AddReplacedNotifications<TComponent>()
        {
            // @formatter:off
            Parent
                .Components<Singletons>()
                    .Add<ComponentReplacedListeners<TScope, TComponent>>()
                .End()
                .Messages()
                    .Add<ComponentReplaced<TScope, TComponent>>()
                .End()
                .Systems()
                    .Setup((contexts, bus) => {
                        var unused = new ComponentReplacedMessageProducer<TScope, TComponent>(contexts, bus);
    
                        contexts.Singleton().Add(new ComponentReplacedListeners<TScope, TComponent>(new List<IComponentReplacedListener<TScope, TComponent>>()));
                    })
                    .Update((contexts, bus) => {
                        var listeners = contexts.Singleton().Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners;
                    
                        if (listeners.Count == 0)
                            return;
        
                        foreach (var message in bus.All<ComponentReplaced<TScope, TComponent>>())
                        {
                            foreach (var listener in listeners)
                            {
                                listener.OnComponentReplaced(message.Entity, message.OldComponent);
                            }
                        }
                    })
                .End();
            // @formatter:on
            return this;
        }

        public ComponentNotificationsBuilder<TScope> AddAllNotifications<TComponent>()
        {
            return AddAddedNotifications<TComponent>()
                .AddRemovedNotifications<TComponent>()
                .AddReplacedNotifications<TComponent>();
        }
    }
}