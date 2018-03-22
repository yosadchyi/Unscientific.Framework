namespace Unscientific.ECS.Unity
{
    public interface IHandler
    {
        void Initialize(Contexts contexts, MessageBus messageBus);
        void Destroy();
    }
}