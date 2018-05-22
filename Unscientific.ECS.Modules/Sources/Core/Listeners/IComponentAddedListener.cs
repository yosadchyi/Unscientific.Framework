namespace Unscientific.ECS.Features.Core
{
    public interface IComponentAddedListener<TScope, in TComponent>
    {
        void OnComponentAdded(Entity<TScope> entity);
    }
}