namespace Unscientificlab.ECS
{
    public interface IComponentListener<TScope, in TComponent> where TScope : IScope
    {
        void OnAdded(Entity<TScope> entity, TComponent component);
        void OnRemoved(Entity<TScope> entity, TComponent component);
        void OnReplaced(Entity<TScope> entity, TComponent oldComponent, TComponent newComponent);
        void OnIndexChanged(Entity<TScope> entity, TComponent component);
    }
}