namespace Unscientific.ECS.Modules.Core
{
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
            _singletonContext.Singleton().Add(new ComponentAddedListeners<TScope, TComponent>());
        }

        public void Update()
        {
            var listeners = _singletonContext.Singleton().Get<ComponentAddedListeners<TScope, TComponent>>().Listeners;
            
            if (listeners.Count == 0)
                return;

            foreach (var message in _messageBus.All<ComponentAdded<TScope, TComponent>>())
            {
                foreach (var listener in listeners)
                {
                    listener.OnComponentAdded(message.Entity, message.Component);
                }
            }
        }
    }
}