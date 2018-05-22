namespace Unscientific.ECS.Features.Core
{
    public class ComponentAddedMessageProducer<TScope, TComponent>: IComponentAddedListener<TScope, TComponent>
    {
        private readonly MessageBus _messageBus;

        public ComponentAddedMessageProducer(Contexts contexts, MessageBus messageBus)
        {
            contexts.Get<TScope>().AddComponentAddedListener<TComponent>(OnComponentAdded);
            _messageBus = messageBus;
        }

        public void OnComponentAdded(Entity<TScope> entity)
        {
            _messageBus.Send(new ComponentAdded<TScope, TComponent>(entity));
        }
    }
}
