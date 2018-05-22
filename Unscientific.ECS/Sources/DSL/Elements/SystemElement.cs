using System;

namespace Unscientific.ECS.DSL
{
    internal class SystemElement
    {
        internal readonly Action<Contexts, MessageBus> Action;

        internal SystemElement(Action<Contexts, MessageBus> action)
        {
            Action = action;
        }
    }
}