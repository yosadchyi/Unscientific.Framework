namespace Unscientific.ECS
{
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
}