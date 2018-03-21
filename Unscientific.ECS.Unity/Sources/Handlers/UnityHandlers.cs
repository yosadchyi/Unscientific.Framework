using UnityEngine;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Unity
{
    public class UnityHandlers
    {
        public RemoveViewHandler<Game> RemoveViewHandler;
        public ViewHandler<Game> ViewHandler;

        public void Initialize(Contexts contexts, MessageBus messageBus, Transform parenTransform)
        {
            RemoveViewHandler = new RemoveViewHandler<Game>(contexts);
            ViewHandler = new ViewHandler<Game>(contexts, parenTransform);
        }

        public void Destroy()
        {
            RemoveViewHandler.Destroy();
            ViewHandler.Destroy();
        }
    }
}