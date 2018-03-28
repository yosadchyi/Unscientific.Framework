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
}