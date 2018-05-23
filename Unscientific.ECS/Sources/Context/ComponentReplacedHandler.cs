namespace Unscientific.ECS
{
    public delegate void ComponentReplacedHandler<TScope, in TComponent>(Entity<TScope> entity, TComponent oldComponent);
}