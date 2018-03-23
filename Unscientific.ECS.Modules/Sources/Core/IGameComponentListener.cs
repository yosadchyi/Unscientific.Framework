namespace Unscientific.ECS.Modules.Core
{
    public interface IGameComponentAddedListener<in TComponent>: IComponentAddedListener<Game, TComponent>
    {
    }

    public interface IGameComponentRemovedListener<in TComponent>: IComponentRemovedListener<Game, TComponent>
    {
    }

    public interface IGameComponentReplacedListener<in TComponent>: IComponentReplacedListener<Game, TComponent>
    {
    }

    public interface IGameComponentListener<in TComponent>: IComponentListener<Game, TComponent>
    {
    }
}