namespace Unscientific.ECS
{
    public class MessageNotRegisteredException<TMessage> : System.Exception
    {
        public MessageNotRegisteredException() : base($"Unregistered message type `{typeof(TMessage).Name}'!")
        {
        }
    }
}