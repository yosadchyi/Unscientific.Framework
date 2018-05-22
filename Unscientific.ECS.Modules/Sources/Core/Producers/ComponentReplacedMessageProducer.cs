namespace Unscientific.ECS.Modules.Core
{
    public class ComponentReplacedMessageProducer<TScope, TComponent>: IComponentReplacedListener<TScope, TComponent>
    {
        private readonly MessageBus _messageBus;

        public ComponentReplacedMessageProducer(Contexts contexts, MessageBus messageBus)
        {
            contexts.Get<TScope>().AddComponentReplacedListener<TComponent>(OnComponentReplaced);
            _messageBus = messageBus;
        }

        public void OnComponentReplaced(Entity<TScope> entity, TComponent oldComponent)
        {
            _messageBus.Send(new ComponentReplaced<TScope, TComponent>(entity, oldComponent));
        }
    }
}