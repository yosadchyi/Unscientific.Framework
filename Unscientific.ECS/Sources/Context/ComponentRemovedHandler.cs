namespace Unscientific.ECS
{
    public delegate void ComponentRemovedHandler<TScope, in TComponent>(Entity<TScope> entity, TComponent component);
}