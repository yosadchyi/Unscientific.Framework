using System;
using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Core
{
    public partial class Module<TTag>
    {
        public partial class Builder
        {
            public class NotificationRegistrationsBuilder<TScope> where TScope : IScope
            {
                // ReSharper disable once StaticMemberInGenericType
                private static readonly HashSet<Type> RegisteredComponentMessageProducers = new HashSet<Type>();

                private readonly Builder _parent;
                private readonly List<Action<Contexts, MessageBus>> _registrations = new List<Action<Contexts, MessageBus>>();
                private readonly List<SystemFactory> _systemFactories = new List<SystemFactory>();

                public NotificationRegistrationsBuilder(Builder parent)
                {
                    _parent = parent;
                }

                public NotificationRegistrationsBuilder<TScope> AddComponentAddedNotifications<TComponent>()
                {
                    _parent._componentRegistrations.For<Singletons>()
                        .Add<ComponentAddedListeners<TScope, TComponent>>()
                        .End();
                    _parent._messageRegistrations.Add<ComponentAdded<TScope, TComponent>>();
                    _systemFactories.Add((contexts, messageBus) => new ComponentAddedNotificationSystem<TScope, Singletons, TComponent>(contexts, messageBus));
                    _registrations.Add((contexts, messageBus) =>
                    {
                        if (RegisteredComponentMessageProducers.Contains(typeof(ComponentAddedMessageProducer<TScope, TComponent>)))
                            return;

                        var unused = new ComponentAddedMessageProducer<TScope, TComponent>(contexts, messageBus);

                        RegisteredComponentMessageProducers.Add(typeof(ComponentAddedMessageProducer<TScope, TComponent>));
                    });
                    return this;
                }

                public NotificationRegistrationsBuilder<TScope> AddComponentRemovedNotifications<TComponent>()
                {
                    _parent._componentRegistrations.For<Singletons>()
                        .Add<ComponentRemovedListeners<TScope, TComponent>>()
                        .End();
                    _parent._messageRegistrations.Add<ComponentRemoved<TScope, TComponent>>();
                    _systemFactories.Add((contexts, messageBus) => new ComponentRemovedNotificationSystem<TScope, Singletons, TComponent>(contexts, messageBus));
                    _registrations.Add((contexts, messageBus) =>
                    {
                        if (RegisteredComponentMessageProducers.Contains(typeof(ComponentRemovedMessageProducer<TScope, TComponent>)))
                            return;

                        var unused = new ComponentRemovedMessageProducer<TScope, TComponent>(contexts, messageBus);

                        RegisteredComponentMessageProducers.Add(typeof(ComponentRemovedMessageProducer<TScope, TComponent>));
                    });
                    return this;
                }

                public NotificationRegistrationsBuilder<TScope> AddComponentReplacedNotifications<TComponent>()
                {
                    _parent._componentRegistrations.For<Singletons>()
                        .Add<ComponentReplacedListeners<TScope, TComponent>>()
                        .End();
                    _parent._messageRegistrations.Add<ComponentReplaced<TScope, TComponent>>();
                    _systemFactories.Add((contexts, messageBus) => new ComponentReplacedNotificationSystem<TScope, Singletons, TComponent>(contexts, messageBus));
                    _registrations.Add((contexts, messageBus) =>
                    {
                        if (RegisteredComponentMessageProducers.Contains(typeof(ComponentReplacedMessageProducer<TScope, TComponent>)))
                            return;

                        var unused = new ComponentReplacedMessageProducer<TScope, TComponent>(contexts, messageBus);

                        RegisteredComponentMessageProducers.Add(typeof(ComponentReplacedMessageProducer<TScope, TComponent>));
                    });
                    return this;
                }

                public NotificationRegistrationsBuilder<TScope> AddComponentNotifications<TComponent>()
                {
                    return AddComponentAddedNotifications<TComponent>()
                        .AddComponentRemovedNotifications<TComponent>()
                        .AddComponentReplacedNotifications<TComponent>();
                }

                public Builder End()
                {
                    _parent._notificationRegistrations.AddAll(new NotificationRegistrations(_registrations));
                    _parent._notificationSystemFactories.AddRange(_systemFactories);
                    return _parent;
                }
            }
        }
    }
}