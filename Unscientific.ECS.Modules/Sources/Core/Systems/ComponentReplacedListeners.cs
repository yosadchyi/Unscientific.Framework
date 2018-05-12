using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Core
{
    public class ComponentReplacedListeners<TScope, TComponent> where TScope : IScope
    {
        public readonly List<IComponentReplacedListener<TScope, TComponent>> Listeners;

        public ComponentReplacedListeners()
        {
            Listeners = new List<IComponentReplacedListener<TScope, TComponent>>();
        }
    }
}