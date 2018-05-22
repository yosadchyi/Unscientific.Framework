namespace Unscientific.ECS.Features.Core
{
    public struct ComponentReplaced<TScope, TComponent>
    {
        public readonly Entity<TScope> Entity;
        public readonly TComponent OldComponent;

        public ComponentReplaced(Entity<TScope> entity, TComponent oldComponent)
        {
            Entity = entity;
            OldComponent = oldComponent;
        }
    }
}