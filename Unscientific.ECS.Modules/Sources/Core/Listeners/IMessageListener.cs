namespace Unscientific.ECS.Modules.Core
{
    public interface IMessageListener<in TMessage>
    {
        void OnMessage(TMessage message);
    }
}
