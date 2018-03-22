using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class UnityHandlers: MonoBehaviour, IHandler
    {
        private List<IHandler> _handlers;

        private void Awake()
        {
            _handlers = GetComponents<IHandler>().Where(handler => !ReferenceEquals(handler, this)).ToList();
            foreach (var handler in _handlers)
            {
                Debug.Log(handler);
            }
        }

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
            foreach (var handler in _handlers)
            {
                handler.Initialize(contexts, messageBus);
            }
        }

        public void Destroy()
        {
            foreach (var handler in _handlers)
            {
                handler.Destroy();
            }
        }
    }
}