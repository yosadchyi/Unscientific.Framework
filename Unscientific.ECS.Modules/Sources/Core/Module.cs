using System;
using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Core
{
    /// <summary>
    /// This class represents ECS module, which is set of components, systems, messages providing given functionality. 
    /// </summary>
    public partial class Module<TTag> : IModule
    {
        private ModuleUsages _usages;
        private ContextRegistrations _contextRegistrations;
        private MessageRegistrations _messageRegistrations;
        private ComponentRegistrations _componentRegistrations;
        private List<SystemFactory> _systemFactories;
        private NotificationRegistrations _notificationRegistrations;

        public partial class Builder : IModuleBuilder
        {
        }

        private Module()
        {
        }

        public ModuleUsages Usages()
        {
            return _usages;
        }

        public ContextRegistrations Contexts()
        {
            return _contextRegistrations;
        }

        public MessageRegistrations Messages()
        {
            return _messageRegistrations;
        }

        public ComponentRegistrations Components()
        {
            return _componentRegistrations;
        }

        public NotificationRegistrations Notifications()
        {
            return _notificationRegistrations;
        }

        public Systems Systems(Contexts contexts, MessageBus bus)
        {
            var builder = new Systems.Builder();

            foreach (var factory in _systemFactories)
            {
                var system = factory(contexts, bus);
                var systems = system as Systems;

                if (systems != null)
                {
                    builder.Add(systems);
                }
                else
                {
                    builder.Add(system);
                }
            }

            return builder.Build();
        }
    }
}