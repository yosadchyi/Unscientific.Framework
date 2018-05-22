using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Core
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