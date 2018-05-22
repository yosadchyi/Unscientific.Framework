namespace Unscientific.ECS.Modules.Core
{
    public interface IComponentListener<TScope, in TComponent>
        : IComponentAddedListener<TScope, TComponent>,
            IComponentRemovedListener<TScope, TComponent>,
            IComponentReplacedListener<TScope, TComponent>
    {
    }
}