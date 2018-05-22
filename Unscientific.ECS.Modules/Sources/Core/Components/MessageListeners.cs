using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Core
{
    public struct MessageListeners<TMessage>
    {
        public readonly List<IMessageListener<TMessage>> Listeners;

        public MessageListeners(List<IMessageListener<TMessage>> listeners)
        {
            Listeners = listeners;
        }
    }
}