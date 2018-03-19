using System;
using System.Collections.Generic;

namespace Unscientific.ECS.Listener
{
    public class MessageProducerRegistrations
    {
        private static readonly HashSet<Type> RegisteredComponentMessageProducers = new HashSet<Type>();

        public class Builder<TParent> where TParent: class 
        {
            private readonly TParent _parent;
            private readonly MessageRegistrations _messageRegistrations;
            private readonly Action<MessageProducerRegistrations> _consumer;
            private readonly List<Action<Contexts, MessageBus>> _registrations = new List<Action<Contexts, MessageBus>>();

            public Builder(TParent parent, MessageRegistrations messageRegistrations, Action<MessageProducerRegistrations> consumer)
            {
                _parent = parent;
                _messageRegistrations = messageRegistrations;
                _consumer = consumer;
            }

            public class ScopedBuilder<TScope> where TScope : IScope
            {
                private readonly Builder<TParent> _builder;

                public ScopedBuilder(Builder<TParent> builder)
                {
                    _builder = builder;
                }

                public ScopedBuilder<TScope> AddComponentAddedMessages<TComponent>()
                {
                    _builder._messageRegistrations.Add<ComponentAdded<TScope, TComponent>>();
                    _builder._registrations.Add((contexts, messageBus) =>
                    {
                        if (RegisteredComponentMessageProducers.Contains(typeof(ComponentAddedMessageProducer<TScope, TComponent>)))
                            return;
                        
                        var unused = new ComponentAddedMessageProducer<TScope, TComponent>(contexts, messageBus);

                        RegisteredComponentMessageProducers.Add(typeof(ComponentAddedMessageProducer<TScope, TComponent>));
                    });
                    return this;
                }

                public ScopedBuilder<TScope> AddComponentRemovedMessages<TComponent>()
                {
                    _builder._messageRegistrations.Add<ComponentRemoved<TScope, TComponent>>();
                    _builder._registrations.Add((contexts, messageBus) =>
                    {
                        if (RegisteredComponentMessageProducers.Contains(typeof(ComponentRemovedMessageProducer<TScope, TComponent>)))
                            return;
                        
                        var unused = new ComponentRemovedMessageProducer<TScope, TComponent>(contexts, messageBus);

                        RegisteredComponentMessageProducers.Add(typeof(ComponentRemovedMessageProducer<TScope, TComponent>));
                    });
                    return this;
                }

                public ScopedBuilder<TScope> AddComponentReplacedMessages<TComponent>()
                {
                    _builder._messageRegistrations.Add<ComponentReplaced<TScope, TComponent>>();
                    _builder._registrations.Add((contexts, messageBus) =>
                    {
                        if (RegisteredComponentMessageProducers.Contains(typeof(ComponentReplacedMessageProducer<TScope, TComponent>)))
                            return;
                        
                        var unused = new ComponentReplacedMessageProducer<TScope, TComponent>(contexts, messageBus);

                        RegisteredComponentMessageProducers.Add(typeof(ComponentReplacedMessageProducer<TScope, TComponent>));
                    });
                    return this;
                }

                public Builder<TParent> End()
                {
                    return _builder;
                }
            }

            public ScopedBuilder<TScope> For<TScope>() where TScope : IScope
            {
                return new ScopedBuilder<TScope>(this);
            }

            public MessageProducerRegistrations Build()
            {
                return new MessageProducerRegistrations(_registrations);
            }
            
            public TParent End()
            {
                _consumer(Build());
                return _parent;
            }
        }
        
        private readonly List<Action<Contexts, MessageBus>> _registrations;

        private MessageProducerRegistrations(List<Action<Contexts, MessageBus>> registrations)
        {
            _registrations = registrations;
        }

        public MessageProducerRegistrations()
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
    }
}
