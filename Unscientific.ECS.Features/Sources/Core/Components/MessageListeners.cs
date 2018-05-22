using System.Collections.Generic;

namespace Unscientific.ECS.Features.Core.Components
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