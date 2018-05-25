using System.Collections.Generic;

namespace Unscientific.ECS.Features.Core
{
    public struct ComponentRemovedListeners<TScope, TComponent>
    {
        public readonly List<IComponentRemovedListener<TScope, TComponent>> Listeners;

        public ComponentRemovedListeners(List<IComponentRemovedListener<TScope, TComponent>> listeners)
        {
            Listeners = listeners;
        }
    }
}