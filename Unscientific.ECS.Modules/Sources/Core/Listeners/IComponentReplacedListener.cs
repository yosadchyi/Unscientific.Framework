namespace Unscientific.ECS.Modules.Core
{
    public interface IComponentReplacedListener<TScope, in TComponent>
    {
        void OnComponentReplaced(Entity<TScope> entity, TComponent oldComponent);
    }
}