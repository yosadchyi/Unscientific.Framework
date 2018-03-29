namespace Unscientific.ECS.Unity
{
    public interface IOrientationHandler<TScope>: IHandler where TScope : IScope
    {
        void UpdateOrientation(Entity<TScope> entity);
    }
}