namespace Unscientific.ECS.Unity
{
    public interface IPositionHandler<TScope>: IHandler where TScope : IScope
    {
        void UpdatePostion(Entity<TScope> entity);
    }
}