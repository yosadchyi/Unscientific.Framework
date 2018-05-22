namespace Unscientific.ECS.Features.Core
{
    public interface IComponentListener<TScope, in TComponent>
        : IComponentAddedListener<TScope, TComponent>,
            IComponentRemovedListener<TScope, TComponent>,
            IComponentReplacedListener<TScope, TComponent>
    {
    }
}