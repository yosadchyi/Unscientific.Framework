using System.Collections.Generic;

namespace Unscientific.ECS.Features.Core.Components
{
    public struct ComponentReplacedListeners<TScope, TComponent>
    {
        public readonly List<IComponentReplacedListener<TScope, TComponent>> Listeners;

        public ComponentReplacedListeners(List<IComponentReplacedListener<TScope, TComponent>> listeners)
        {
            Listeners = listeners;
        }
    }
}