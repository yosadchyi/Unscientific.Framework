using System;
using System.Collections.Generic;

namespace Unscientific.ECS
{
    public class NotificationRegistrations
    {
        private readonly List<Action<Contexts, MessageBus>> _registrations;

        public NotificationRegistrations(List<Action<Contexts, MessageBus>> registrations)
        {
            _registrations = registrations;
        }

        public NotificationRegistrations()
        {
            _registrations = new List<Action<Contexts, MessageBus>>();
        }

        public void Register(Contexts contexts, MessageBus messageBus)
        {
            foreach (var registration in _registrations)
            {
                registration(contexts, messageBus);
            }
        }

        public void AddAll(NotificationRegistrations registrations)
        {
            _registrations.AddRange(registrations._registrations);
        }
    }
}