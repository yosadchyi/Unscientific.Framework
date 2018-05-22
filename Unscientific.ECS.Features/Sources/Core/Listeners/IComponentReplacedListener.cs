namespace Unscientific.ECS.Features.Core
{
    public interface IComponentReplacedListener<TScope, in TComponent>
    {
        void OnComponentReplaced(Entity<TScope> entity, TComponent oldComponent);
    }
}