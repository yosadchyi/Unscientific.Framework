using System.Collections.Generic;

namespace Unscientific.ECS
{
    public class ComponentAddedListeners<TScope, TComponent> where TScope : IScope
    {
        public readonly List<IComponentAddedListener<TScope, TComponent>> Listeners;

        public ComponentAddedListeners()
        {
            Listeners = new List<IComponentAddedListener<TScope, TComponent>>();
        }
    }

    public class ComponentRemovedListeners<TScope, TComponent> where TScope : IScope
    {
        public readonly List<IComponentRemovedListener<TScope, TComponent>> Listeners;

        public ComponentRemovedListeners()
        {
            Listeners = new List<IComponentRemovedListener<TScope, TComponent>>();
        }
    }

    public class ComponentReplacedListeners<TScope, TComponent> where TScope : IScope
    {
        public readonly List<IComponentReplacedListener<TScope, TComponent>> Listeners;

        public ComponentReplacedListeners()
        {
            Listeners = new List<IComponentReplacedListener<TScope, TComponent>>();
        }
    }

    public class ComponentAddedNotificationSystem<TScope, TSingletonScope, TComponent>: ISetupSystem, IUpdateSystem
        where TScope : IScope
        where TSingletonScope : IScope
    {
        private readonly MessageBus _messageBus;
        private readonly Context<TSingletonScope> _singletonContext;

        public ComponentAddedNotificationSystem(Contexts contexts, MessageBus messageBus)
        {
            _singletonContext = contexts.Get<TSingletonScope>();
            _messageBus = messageBus;
        }

        public void Setup()
        {
            _singletonContext.First().Add(new ComponentAddedListeners<TScope, TComponent>());
        }

        public void Update()
        {
            var listeners = _singletonContext.First().Get<ComponentAddedListeners<TScope, TComponent>>().Listeners;

            foreach (var message in _messageBus.All<ComponentAdded<TScope, TComponent>>())
            {
                foreach (var listener in listeners)
                {
                    listener.OnComponentAdded(message.Entity, message.Component);
                }
            }
        }
    }
    
    public class ComponentRemovedNotificationSystem<TScope, TSingletonScope, TComponent>: ISetupSystem, IUpdateSystem
        where TScope : IScope
        where TSingletonScope : IScope
    {
        private readonly MessageBus _messageBus;
        private readonly Context<TSingletonScope> _singletonContext;

        public ComponentRemovedNotificationSystem(Contexts contexts, MessageBus messageBus)
        {
            _singletonContext = contexts.Get<TSingletonScope>();
            _messageBus = messageBus;
        }

        public void Setup()
        {
            _singletonContext.First().Add(new ComponentRemovedListeners<TScope, TComponent>());
        }

        public void Update()
        {
            var listeners = _singletonContext.First().Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners;

            foreach (var message in _messageBus.All<ComponentRemoved<TScope, TComponent>>())
            {
                foreach (var listener in listeners)
                {
                    listener.OnComponentRemoved(message.Entity, message.Component);
                }
            }
        }
    }

    public class ComponentReplacedNotificationSystem<TScope, TSingletonScope, TComponent>: ISetupSystem, IUpdateSystem
        where TScope : IScope
        where TSingletonScope : IScope
    {
        private readonly MessageBus _messageBus;
        private readonly Context<TSingletonScope> _singletonContext;

        public ComponentReplacedNotificationSystem(Contexts contexts, MessageBus messageBus)
        {
            _singletonContext = contexts.Get<TSingletonScope>();
            _messageBus = messageBus;
        }

        public void Setup()
        {
            _singletonContext.First().Add(new ComponentReplacedListeners<TScope, TComponent>());
        }

        public void Update()
        {
            var listeners = _singletonContext.First().Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners;

            foreach (var message in _messageBus.All<ComponentReplaced<TScope, TComponent>>())
            {
                foreach (var listener in listeners)
                {
                    listener.OnComponentReplaced(message.Entity, message.OldComponent, message.NewComponent);
                }
            }
        }
    }

}