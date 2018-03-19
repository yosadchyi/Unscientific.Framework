namespace Unscientific.ECS
{
    public interface IComponentAddedListener<TScope, in TComponent> where TScope : IScope
    {
        void OnComponentAdded(Entity<TScope> entity, TComponent component);
    }

    public interface IComponentRemovedListener<TScope, in TComponent> where TScope : IScope
    {
        void OnComponentRemoved(Entity<TScope> entity, TComponent component);
    }

    public interface IComponentReplacedListener<TScope, in TComponent> where TScope : IScope
    {
        void OnComponentReplaced(Entity<TScope> entity, TComponent oldComponent, TComponent newComponent);
    }

    public interface IComponentListener<TScope, in TComponent>
        : IComponentAddedListener<TScope, TComponent>,
            IComponentRemovedListener<TScope, TComponent>,
            IComponentReplacedListener<TScope, TComponent> where TScope : IScope
    {
    }
}