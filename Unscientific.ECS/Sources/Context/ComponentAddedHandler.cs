namespace Unscientific.ECS
{
    public delegate void ComponentAddedHandler<TScope, in TComponent>(Entity<TScope> entity);
}