using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Core
{
    public class ComponentRemovedListeners<TScope, TComponent> where TScope : IScope
    {
        public readonly List<IComponentRemovedListener<TScope, TComponent>> Listeners;

        public ComponentRemovedListeners()
        {
            Listeners = new List<IComponentRemovedListener<TScope, TComponent>>();
        }
    }
}