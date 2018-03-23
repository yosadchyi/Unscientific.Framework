namespace Unscientific.ECS
{
    public interface IMessageListener<in TMessage>
    {
        void OnMessage(TMessage message);
    }
}
