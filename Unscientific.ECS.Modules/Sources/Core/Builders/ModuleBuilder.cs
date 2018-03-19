using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Core
{
    public partial class Module<TTag>
    {
        public partial class Builder : IModuleBuilder
        {
            private readonly List<SystemFactory> _systemFactories = new List<SystemFactory>();
            private readonly List<SystemFactory> _notificationSystemFactories = new List<SystemFactory>();
            private readonly ModuleUsages _usages = new ModuleUsages();
            private readonly ContextRegistrations _contextRegistrations = new ContextRegistrations();
            private readonly MessageRegistrations _messageRegistrations = new MessageRegistrations();
            private readonly ComponentRegistrations _componentRegistrations = new ComponentRegistrations();
            private readonly NotificationRegistrations _notificationRegistrations = new NotificationRegistrations();

            public SystemsBuilder Systems()
            {
                return new SystemsBuilder(this);
            }

            public ModuleUsagesBuilder Usages()
            {
                return new ModuleUsagesBuilder(this);
            }

            public MessageRegistrationsBuilder Messages()
            {
                return new MessageRegistrationsBuilder(this);
            }

            public NotificationRegistrationsBuilder<TScope> Notifications<TScope>() where TScope : IScope
            {
                return new NotificationRegistrationsBuilder<TScope>(this);
            }

            public ContextRegistrationsBuilder Contexts()
            {
                return new ContextRegistrationsBuilder(this);
            }

            public ComponentRegistrationsBuilder<TScope> Components<TScope>() where TScope : IScope
            {
                return new ComponentRegistrationsBuilder<TScope>(this);
            }

            public IModule Build()
            {
                _systemFactories.AddRange(_notificationSystemFactories);
                return new Module<TTag>
                {
                    _usages = _usages,
                    _contextRegistrations = _contextRegistrations,
                    _messageRegistrations = _messageRegistrations,
                    _componentRegistrations = _componentRegistrations,
                    _notificationRegistrations = _notificationRegistrations,
                    _systemFactories = _systemFactories,
                };
            }
        }

    }
}