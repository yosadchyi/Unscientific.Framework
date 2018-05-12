namespace Unscientific.ECS.Modules.Core
{
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
            _singletonContext.Singleton().Add(new ComponentRemovedListeners<TScope, TComponent>());
        }

        public void Update()
        {
            var listeners = _singletonContext.Singleton().Get<ComponentRemovedListeners<TScope, TComponent>>().Listeners;
            
            if (listeners.Count == 0)
                return;

            foreach (var message in _messageBus.All<ComponentRemoved<TScope, TComponent>>())
            {
                foreach (var listener in listeners)
                {
                    listener.OnComponentRemoved(message.Entity, message.Component);
                }
            }
        }
    }
}