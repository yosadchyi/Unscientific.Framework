namespace Unscientific.ECS.Unity
{
    public interface IPositionHandler<TScope>: IHandler
    {
        void UpdatePostion(Entity<TScope> entity);
    }
}