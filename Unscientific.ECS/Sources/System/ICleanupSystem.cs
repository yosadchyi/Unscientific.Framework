namespace Unscientific.ECS
{
    public interface ICleanupSystem: ISystem
    {
        void Cleanup();
    }
}