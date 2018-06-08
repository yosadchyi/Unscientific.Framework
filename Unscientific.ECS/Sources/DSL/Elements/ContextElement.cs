using System;

namespace Unscientific.ECS.DSL
{
    internal class ContextElement
    {
        internal readonly Type ScopeType;
        internal readonly int InitialCapacity;
        internal readonly int MaxCapacity;
        internal readonly Func<ContextInfo, IContext> ContextCtor;

        internal ContextElement(Type scopeType, int initialCapacity, int maxCapacity, Func<ContextInfo, IContext> contextCtor)
        {
            ScopeType = scopeType;
            InitialCapacity = initialCapacity;
            MaxCapacity = maxCapacity;
            ContextCtor = contextCtor;
        }
    }
}