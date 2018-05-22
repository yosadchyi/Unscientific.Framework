namespace Unscientific.ECS
{
    public interface ISystems
    {
        void Setup(Contexts contexts, MessageBus messageBus);
        void Update(Contexts contexts, MessageBus messageBus);
        void Cleanup(Contexts contexts, MessageBus messageBus);
    }
}