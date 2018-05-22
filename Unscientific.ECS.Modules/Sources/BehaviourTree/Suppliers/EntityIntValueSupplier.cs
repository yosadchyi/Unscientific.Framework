using System;

namespace Unscientific.ECS.Features.BehaviourTree
{
    public class EntityIntValueSupplier<TScope, TComponent> : EntityValueSupplier<TScope, TComponent, int>
    {
        public EntityIntValueSupplier(Func<TComponent, int> valueExtractor) : base(valueExtractor)
        {
        }
    }
}