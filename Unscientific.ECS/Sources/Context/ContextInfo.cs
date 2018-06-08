using System;
using System.Collections.Generic;

namespace Unscientific.ECS
{
    internal class ContextInfo
    {
        internal List<Func<ComponentInfo>> ComponentCtors;
        internal int InitialCapacity;
        internal int MaxCapacity;
    }
}