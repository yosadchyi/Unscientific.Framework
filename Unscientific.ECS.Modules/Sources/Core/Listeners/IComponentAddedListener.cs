namespace Unscientific.ECS.Modules.Core
{
    public interface IComponentAddedListener<TScope, in TComponent>
    {
        void OnComponentAdded(Entity<TScope> entity);
    }
}