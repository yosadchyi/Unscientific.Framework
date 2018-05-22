using System;

namespace Unscientific.ECS.DSL
{
    internal class ContextElement
    {
        internal readonly Type ScopeType;
        internal readonly int InitialCapacity;
        internal readonly int MaxCapacity;

        internal ContextElement(Type scopeType, int initialCapacity, int maxCapacity)
        {
            ScopeType = scopeType;
            InitialCapacity = initialCapacity;
            MaxCapacity = maxCapacity;
        }
    }
}