namespace Unscientific.ECS.Features.Core
{
    public interface IMessageListener<in TMessage>
    {
        void OnMessage(TMessage message);
    }
}
