namespace Unscientific.ECS.Features.Core
{
    public class ComponentRemovedMessageProducer<TScope, TComponent>: IComponentRemovedListener<TScope, TComponent>
    {
        private readonly MessageBus _messageBus;

        public ComponentRemovedMessageProducer(Contexts contexts, MessageBus messageBus)
        {
            contexts.Get<TScope>().AddComponentRemovedListener<TComponent>(OnComponentRemoved);
            _messageBus = messageBus;
        }

        public void OnComponentRemoved(Entity<TScope> entity, TComponent component)
        {
            _messageBus.Send(new ComponentRemoved<TScope, TComponent>(entity, component));
        }
    }
}