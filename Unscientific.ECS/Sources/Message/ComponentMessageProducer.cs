namespace Unscientific.ECS
{
    public struct ComponentAdded<TScope, TComponent> where TScope : IScope
    {
        public readonly Entity<TScope> Entity;
        public readonly TComponent Component;

        public ComponentAdded(Entity<TScope> entity, TComponent component)
        {
            Entity = entity;
            Component = component;
        }
    }

    public struct ComponentRemoved<TScope, TComponent> where TScope : IScope
    {
        public readonly Entity<TScope> Entity;
        public readonly TComponent Component;

        public ComponentRemoved(Entity<TScope> entity, TComponent component)
        {
            Entity = entity;
            Component = component;
        }
    }

    public struct ComponentReplaced<TScope, TComponent> where TScope : IScope
    {
        public readonly Entity<TScope> Entity;
        public readonly TComponent OldComponent;
        public readonly TComponent NewComponent;

        public ComponentReplaced(Entity<TScope> entity, TComponent oldComponent, TComponent newComponent)
        {
            Entity = entity;
            OldComponent = oldComponent;
            NewComponent = newComponent;
        }
    }

    public class ComponentAddedMessageProducer<TScope, TComponent>: IComponentAddedListener<TScope, TComponent> where TScope : IScope
    {
        private readonly MessageBus _messageBus;

        public ComponentAddedMessageProducer(Contexts contexts, MessageBus messageBus)
        {
            contexts.Get<TScope>().AddComponentAddedListener<TComponent>(OnComponentAdded);
            _messageBus = messageBus;
        }

        public void OnComponentAdded(Entity<TScope> entity, TComponent component)
        {
            _messageBus.Send(new ComponentAdded<TScope, TComponent>(entity, component));
        }
    }

    public class ComponentRemovedMessageProducer<TScope, TComponent>: IComponentRemovedListener<TScope, TComponent> where TScope : IScope
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
