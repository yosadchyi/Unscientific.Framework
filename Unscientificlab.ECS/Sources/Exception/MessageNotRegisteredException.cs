namespace Unscientificlab.ECS
{
    public class MessageNotRegisteredException<TMessage> : global::System.Exception
    {
        public MessageNotRegisteredException() : base(string.Format("Unregistered message type `{0}'!", typeof(TMessage).Name))
        {
        }
    }
}