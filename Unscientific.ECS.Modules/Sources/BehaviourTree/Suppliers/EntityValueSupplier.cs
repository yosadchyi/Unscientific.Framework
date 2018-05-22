using System;
using Unscientific.BehaviourTree;

namespace Unscientific.ECS.Features.BehaviourTree
{
    public class EntityValueSupplier<TScope, TComponent, TType>: IValueSupplier<Entity<TScope>, TType>
    {
        private readonly Func<TComponent, TType> _valueExtractor;

        public EntityValueSupplier(Func<TComponent, TType> valueExtractor)
        {
            _valueExtractor = valueExtractor;
        }

        public TType Supply(Entity<TScope> blackboard)
        {
            return _valueExtractor(blackboard.Get<TComponent>());
        }
    }
}