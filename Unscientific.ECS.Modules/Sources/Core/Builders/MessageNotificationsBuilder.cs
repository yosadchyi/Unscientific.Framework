using System.Collections.Generic;
using Unscientific.ECS.DSL;

namespace Unscientific.ECS.Modules.Core
{
    public class MessageNotificationsBuilder: NestedBuilder<FeatureBuilder>
    {
        public MessageNotificationsBuilder(FeatureBuilder parent) : base(parent)
        {
        }

        public MessageNotificationsBuilder AddMessageNotifications<TMessage>()
        {
            // @formatter:off
            Parent
                .Components<Singletons>()
                .Add<MessageListeners<TMessage>>()
                .End()
                .Systems()
                .Setup((contexts, bus) => {
                    contexts.Singleton().Add(new MessageListeners<TMessage>(new List<IMessageListener<TMessage>>()));
                })
                .Update((contexts, bus) => {
                    var listeners = contexts.Singleton().Get<MessageListeners<TMessage>>().Listeners;
            
                    if (listeners.Count == 0)
                        return;
            
                    foreach (var message in bus.All<TMessage>())
                    {
                        foreach (var listener in listeners)
                        {
                            listener.OnMessage(message);
                        }
                    }
                })
                .End()
                .End();
            // @formatter:on
            return this;
        }
    }
}