namespace Unscientific.ECS
{
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
}