using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Core
{
    public class ComponentAddedListeners<TScope, TComponent> where TScope : IScope
    {
        public readonly List<IComponentAddedListener<TScope, TComponent>> Listeners;

        public ComponentAddedListeners()
        {
            Listeners = new List<IComponentAddedListener<TScope, TComponent>>();
        }
    }
}