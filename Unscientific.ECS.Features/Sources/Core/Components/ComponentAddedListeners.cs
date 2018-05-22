using System.Collections.Generic;

namespace Unscientific.ECS.Features.Core.Components
{
    public struct ComponentAddedListeners<TScope, TComponent>
    {
        public readonly List<IComponentAddedListener<TScope, TComponent>> Listeners;

        public ComponentAddedListeners(List<IComponentAddedListener<TScope, TComponent>> listeners)
        {
            Listeners = listeners;
        }
    }
}