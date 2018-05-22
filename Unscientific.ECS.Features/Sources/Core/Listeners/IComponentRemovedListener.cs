namespace Unscientific.ECS.Features.Core
{
    public interface IComponentRemovedListener<TScope, in TComponent>
    {
        void OnComponentRemoved(Entity<TScope> entity, TComponent component);
    }
}