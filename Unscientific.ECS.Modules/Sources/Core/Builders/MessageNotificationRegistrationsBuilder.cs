using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Core
{
    public partial class Module<TTag>
    {
        public partial class Builder
        {
            public class MessageNotificationRegistrationsBuilder
            {
                private readonly Builder _parent;
                private readonly List<SystemFactory> _systemFactories = new List<SystemFactory>();

                public MessageNotificationRegistrationsBuilder(Builder parent)
                {
                    _parent = parent;
                }

                public MessageNotificationRegistrationsBuilder AddMessageNotification<TMessage>()
                {
                    _parent._componentRegistrations.For<Singletons>()
                        .Add<MessageListeners<TMessage>>()
                        .End();
                    _systemFactories.Add((contexts, messageBus) => new MessageNotificationSystem<Singletons, TMessage>(contexts, messageBus));
                    return this;
                }

                public Builder End()
                {
                    _parent._notificationSystemFactories.AddRange(_systemFactories);
                    return _parent;
                }
            }
        }
    }
}