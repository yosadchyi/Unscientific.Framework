namespace Unscientific.ECS.Modules.Core
{
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
            _singletonContext.Singleton().Add(new ComponentReplacedListeners<TScope, TComponent>());
        }

        public void Update()
        {
            var listeners = _singletonContext.Singleton().Get<ComponentReplacedListeners<TScope, TComponent>>().Listeners;
            
            if (listeners.Count == 0)
                return;

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