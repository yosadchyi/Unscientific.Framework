namespace Unscientific.ECS
{
    public class ComponentReplacedMessageProducer<TScope, TComponent>: IComponentReplacedListener<TScope, TComponent> where TScope : IScope
    {
        private readonly MessageBus _messageBus;

        public ComponentReplacedMessageProducer(Contexts contexts, MessageBus messageBus)
        {
            contexts.Get<TScope>().AddComponentReplacedListener<TComponent>(OnComponentReplaced);
            _messageBus = messageBus;
        }

        public void OnComponentReplaced(Entity<TScope> entity, TComponent oldComponent, TComponent newComponent)
        {
            _messageBus.Send(new ComponentReplaced<TScope, TComponent>(entity, oldComponent, newComponent));
        }
    }
}