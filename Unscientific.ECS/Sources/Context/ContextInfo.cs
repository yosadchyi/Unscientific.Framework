using System;
using System.Collections.Generic;

namespace Unscientific.ECS
{
    internal class ContextInfo
    {
        internal Type ScopeType;
        internal List<Type> Components;
        internal int InitialCapacity;
        internal int MaxCapacity;
    }
}