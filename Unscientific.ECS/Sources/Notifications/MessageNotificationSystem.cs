﻿using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;

namespace Unscientific.ECS
{
    public class MessageListeners<TMessage>
    {
        public readonly List<IMessageListener<TMessage>> Listeners;

        public MessageListeners()
        {
            Listeners = new List<IMessageListener<TMessage>>();
        }
    }

    public class MessageNotificationSystem<TSingletonScope, TMessage> : ISetupSystem, IUpdateSystem where TSingletonScope : IScope
    {
        private readonly MessageBus _messageBus;
        private readonly Context<TSingletonScope> _singletonContext;

        public MessageNotificationSystem(Contexts contexts, MessageBus messageBus)
        {
            _singletonContext = contexts.Get<TSingletonScope>();
            _messageBus = messageBus;
        }

        public void Setup()
        {
            _singletonContext.First().Add(new MessageListeners<TMessage>());
        }

        public void Update()
        {
            var listeners = _singletonContext.First().Get<MessageListeners<TMessage>>().Listeners;

            if (listeners.Count == 0)
                return;
            
            foreach (var message in _messageBus.All<TMessage>())
            {
                foreach (var listener in listeners)
                {
                    listener.OnMessage(message);
                }
            }
        }
    }
}