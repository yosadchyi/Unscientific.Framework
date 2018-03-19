using System;
using System.Collections.Generic;
using Unscientific.ECS.Listener;

namespace Unscientific.ECS
{
    public delegate ISystem SystemFactory(Contexts contexts, MessageBus messageBus);
    
    /// <summary>
    /// This class represents ECS module, which is set of components, systems, messages providing given functionality. 
    /// </summary>
    /// <typeparam name="TTag">Tagging type, should implement IModuleTag interface.</typeparam>
    public class Module<TTag>: IModule
    {
        private readonly ModuleUsages _moduleUsages;
        private readonly ContextRegistrations _contextRegistrations;
        private readonly MessageRegistrations _messageRegistrations;
        private readonly ComponentRegistrations _componentRegistrations;
        private readonly List<SystemFactory> _systemFactories;
        private readonly MessageProducerRegistrations _messageProducerRegistrations;

        public class Builder: IModuleBuilder
        {
            private readonly List<SystemFactory> _systemFactories = new List<SystemFactory>();
            private readonly ModuleUsages _usages = new ModuleUsages();
            private readonly ContextRegistrations _contextRegistrations = new ContextRegistrations();
            private readonly MessageRegistrations _messageRegistrations = new MessageRegistrations();
            private readonly ComponentRegistrations _componentRegistrations = new ComponentRegistrations();
            private MessageProducerRegistrations _messageProducerRegistrations = new MessageProducerRegistrations();

            public class ModuleUsagesBuilder
            {
                private readonly Builder _builder;

                internal ModuleUsagesBuilder(Builder builder)
                {
                    _builder = builder;
                }

                public ModuleUsagesBuilder Uses<TModule>()
                {
                    var iModuleType = typeof(IModule);
                    var iModuleTagType = typeof(IModuleTag);
                    var type = typeof(TModule);

                    if (iModuleTagType.IsAssignableFrom(type))
                    {
                        _builder._usages.Uses<Module<TModule>>();
                    }
                    else if (iModuleType.IsAssignableFrom(type))
                    {
                        _builder._usages.Uses<TModule>();
                    }
                    else
                    {
                        throw new ArgumentException("Uses can accept only types conforming to IModule or IModuleTag interfaces!");
                    }
                    return this;
                }

                public Builder End()
                {
                    return _builder;
                }
            }

            public class MessageRegistrationsBuilder
            {
                private readonly Builder _builder;

                internal MessageRegistrationsBuilder(Builder builder)
                {
                    _builder = builder;
                }

                public MessageRegistrationsBuilder Add<TMessage>(
                    IMessageAggregator<TMessage> aggregator = null,
                    int initialCapacity = 128)
                {
                    _builder._messageRegistrations.Add(aggregator, initialCapacity);
                    return this;
                }

                public MessageRegistrationsBuilder AddDelayed<TMessage>(
                    IMessageAggregator<TMessage> aggregator = null,
                    int initialCapacity = 128)
                {
                    _builder._messageRegistrations.AddDelayed(aggregator, initialCapacity);
                    return this;
                }

                public Builder End()
                {
                    return _builder;
                }
            }

            public class ComponentRegistrationsBuilder<TScope> where TScope : IScope
            {
                private readonly Builder _builder;

                public ComponentRegistrationsBuilder(Builder builder)
                {
                    _builder = builder;
                }

                public ComponentRegistrationsBuilder<TScope> Add<TComponent>()
                {
                    _builder._componentRegistrations
                        .For<TScope>()
                            .Add<TComponent>()
                        .End();
                    return this;
                }

                public Builder End()
                {
                    return _builder;
                }
            }

            public class ContextRegistrationsBuilder
            {
                private readonly Builder _builder;

                internal ContextRegistrationsBuilder(Builder builder)
                {
                    _builder = builder;
                }

                public ContextRegistrationsBuilder Add<TScope>() where TScope : IScope
                {
                    _builder._contextRegistrations.Add<TScope>();
                    return this;
                }

                public Builder End()
                {
                    return _builder;
                }
            }
            
            public class SystemsBuilder
            {
                private readonly Builder _builder;

                internal SystemsBuilder(Builder builder)
                {
                    _builder = builder;
                }
            
                public SystemsBuilder Add(SystemFactory factory)
                {
                    _builder._systemFactories.Add(factory);
                    return this;
                }

                public Builder End()
                {
                    return _builder;
                }
            }

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

            public MessageProducerRegistrations.Builder<Builder> MessageProducers()
            {
                return new MessageProducerRegistrations.Builder<Builder>(this, _messageRegistrations,
                    registrations => _messageProducerRegistrations = registrations);
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
                return new Module<TTag>(_usages,
                    _contextRegistrations,
                    _messageRegistrations,
                    _componentRegistrations,
                    _messageProducerRegistrations,
                    _systemFactories);
            }
        }

        private Module(ModuleUsages moduleUsages,
            ContextRegistrations contextRegistrations,
            MessageRegistrations messageRegistrations,
            ComponentRegistrations componentRegistrations,
            MessageProducerRegistrations messageProducerRegistrations,
            List<SystemFactory> systemFactories)
        {
            _moduleUsages = moduleUsages;
            _contextRegistrations = contextRegistrations;
            _messageRegistrations = messageRegistrations;
            _componentRegistrations = componentRegistrations;
            _messageProducerRegistrations = messageProducerRegistrations;
            _systemFactories = systemFactories;
        }

        public ModuleUsages Usages()
        {
            return _moduleUsages;
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

        public MessageProducerRegistrations MessageProducers()
        {
            return _messageProducerRegistrations;
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