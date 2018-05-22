using System;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class EntityIntValueSupplier<TScope, TComponent> : EntityValueSupplier<TScope, TComponent, int>
    {
        public EntityIntValueSupplier(Func<TComponent, int> valueExtractor) : base(valueExtractor)
        {
        }
    }
}