using System.Collections.Generic;

namespace Unscientific.ECS
{
    public class ComponentAddedListeners<TScope, TComponent> where TScope : IScope
    {
        public List<IComponentAddedListener<TScope, TComponent>> Listeners;

        public ComponentAddedListeners()
        {
            Listeners = new List<IComponentAddedListener<TScope, TComponent>>();
        }
    }

    public class ComponentRemovedListeners<TScope, TComponent> where TScope : IScope
    {
        public List<IComponentRemovedListener<TScope, TComponent>> Listeners;

        public ComponentRemovedListeners()
        {
            Listeners = new List<IComponentRemovedListener<TScope, TComponent>>();
        }
    }

    public class ComponentReplacedListeners<TScope, TComponent> where TScope : IScope
    {
        public List<IComponentReplacedListener<TScope, TComponent>> Listeners;

        public ComponentReplacedListeners()
        {
            Listeners = new List<IComponentReplacedListener<TScope, TComponent>>();
        }
    }

    public class ComponentAddedNotificationSystem<TScope, TSingletonScope, TComponent>: IUpdateSystem
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
    
    public class ComponentRemovedNotificationSystem<TScope, TSingletonScope, TComponent>: IUpdateSystem
        where TScope : IScope
        where TSingletonScope : IScope
    {
        private readonly MessageBus _messageBus;
        private Context<TScope> _entityContext;
        private Context<TSingletonScope> _singletonContext;

        public ComponentRemovedNotificationSystem(Contexts contexts, MessageBus messageBus)
        {
            _entityContext = contexts.Get<TScope>();
            _singletonContext = contexts.Get<TSingletonScope>();
            _messageBus = messageBus;
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

    public class ComponentReplacedNotificationSystem<TScope, TSingletonScope, TComponent>: IUpdateSystem
        where TScope : IScope
        where TSingletonScope : IScope
    {
        private readonly MessageBus _messageBus;
        private Context<TScope> _entityContext;
        private Context<TSingletonScope> _singletonContext;

        public ComponentReplacedNotificationSystem(Contexts contexts, MessageBus messageBus)
        {
            _entityContext = contexts.Get<TScope>();
            _singletonContext = contexts.Get<TSingletonScope>();
            _messageBus = messageBus;
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