using UnityEngine;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Unity
{
    public class UnityHandlers
    {
        public ViewHandler<Game> ViewHandler;

        public void Initialize(Contexts contexts, MessageBus messageBus, Transform parenTransform)
        {
            ViewHandler = new ViewHandler<Game>(contexts, parenTransform);
        }

        public void Destroy()
        {
            ViewHandler.Destroy();
        }
    }
}