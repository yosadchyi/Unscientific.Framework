using Unscientific.ECS.DSL;
using Unscientific.Util.Pool;

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
                .Components<TScope>()
                    .Add<ComponentAddedListeners<TScope, TComponent>>()
                .End()
                .Messages()
                    .Add<ComponentAdded<TScope, TComponent>>()
                .End()
                .Systems()
                    .Setup((contexts, bus) => {
                        var unused = new ComponentAddedMessageProducer<TScope, TComponent>(contexts, bus);
                    })
                    .Update((contexts, bus) => {
                        foreach (var message in bus.All<ComponentAdded<TScope, TComponent>>())
                        {
                            var entity = message.Entity;

                            if (!entity.Has<ComponentAddedListeners<TScope, TComponent>>()) continue;
                            
                            var listeners = entity.Get<ComponentAddedListeners<TScope, TComponent>>().Listeners;

                            foreach (var listener in listeners)
                            {
                                listener.OnComponentAdded(message.Entity);
                            }
                        }
                    })
                    .Cleanup((contexts, bus) =>
                    {
                        var context = contexts.Get<Game>();
    
                        foreach (var entity in context.AllWith<Destroyed, ComponentAddedListeners<TScope, TComponent>>())
                        {
                            var list = entity.Get<ComponentAddedListeners<TScope, TComponent>>().Listeners;

                            ListPool<IComponentAddedListener<TScope, TComponent>>.Instance.Return(list);
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
                .Components<TScope>()
                    .Add<ComponentRemovedListeners<TScope, TComponent>>()
                .End()
                .Messages()
                    .Add<ComponentRemoved<TScope, TComponent>>()
                .End()
                .Systems()
                    .Setup((contexts, bus) => {
                        var unused = new ComponentRemovedMessageProducer<TScope, TComponent>(contexts, bus);
                    })
                    .Update((contexts, bus) => {
                        foreach (var message in bus.All<ComponentRemoved<TScope, TComponent>>())
                        {
                            var entity = message.Entity;

                            if (!entity.Has<ComponentRemovedListeners<TScope, TComponent>>()) continue;

                            var listeners = entity.Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners;

                            foreach (var listener in listeners)
                            {
                                listener.OnComponentRemoved(message.Entity, message.Component);
                            }
                        }
                    })
                    .Cleanup((contexts, bus) =>
                    {
                        var context = contexts.Get<Game>();
    
                        foreach (var entity in context.AllWith<Destroyed, ComponentRemovedListeners<TScope, TComponent>>())
                        {
                            var list = entity.Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners;

                            ListPool<IComponentRemovedListener<TScope, TComponent>>.Instance.Return(list);
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
                .Components<TScope>()
                    .Add<ComponentReplacedListeners<TScope, TComponent>>()
                .End()
                .Messages()
                    .Add<ComponentReplaced<TScope, TComponent>>()
                .End()
                .Systems()
                    .Setup((contexts, bus) => {
                        var unused = new ComponentReplacedMessageProducer<TScope, TComponent>(contexts, bus);
                    })
                    .Update((contexts, bus) => {
                        foreach (var message in bus.All<ComponentReplaced<TScope, TComponent>>())
                        {
                            var entity = message.Entity;

                            if (!entity.Has<ComponentReplacedListeners<TScope, TComponent>>()) continue;
                            
                            var listeners = entity.Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners;

                            foreach (var listener in listeners)
                            {
                                listener.OnComponentReplaced(entity, message.OldComponent);
                            }
                        }
                    })
                    .Cleanup((contexts, bus) =>
                    {
                        var context = contexts.Get<Game>();
    
                        foreach (var entity in context.AllWith<Destroyed, ComponentReplacedListeners<TScope, TComponent>>())
                        {
                            var list = entity.Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners;

                            ListPool<IComponentReplacedListener<TScope, TComponent>>.Instance.Return(list);
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