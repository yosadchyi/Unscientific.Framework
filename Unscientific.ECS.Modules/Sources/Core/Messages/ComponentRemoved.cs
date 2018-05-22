namespace Unscientific.ECS.Features.Core
{
    public struct ComponentRemoved<TScope, TComponent>
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