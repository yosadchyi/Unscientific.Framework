namespace Unscientificlab.ECS
{
    public class MessageNotRegisteredException<TMessage> : System.Exception
    {
        public MessageNotRegisteredException() : base(string.Format("Unregistered message type `{0}'!", typeof(TMessage).Name))
        {
        }
    }
}