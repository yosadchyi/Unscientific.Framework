namespace Unscientific.ECS
{
    public class ComponentAddedMessageProducer<TScope, TComponent>: IComponentAddedListener<TScope, TComponent> where TScope : IScope
    {
        private readonly MessageBus _messageBus;

        public ComponentAddedMessageProducer(Contexts contexts, MessageBus messageBus)
        {
            contexts.Get<TScope>().AddComponentAddedListener<TComponent>(OnComponentAdded);
            _messageBus = messageBus;
        }

        public void OnComponentAdded(Entity<TScope> entity, TComponent view)
        {
            _messageBus.Send(new ComponentAdded<TScope, TComponent>(entity, view));
        }
    }
}
