namespace Unscientific.ECS.Unity
{
    public interface IOrientationHandler<TScope>: IHandler
    {
        void UpdateOrientation(Entity<TScope> entity);
    }
}