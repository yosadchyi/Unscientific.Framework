using System;

namespace Unscientific.ECS.DSL
{
    internal class ComponentElement
    {
        internal readonly Type ScopeType;
        internal readonly Type ComponentType;

        internal ComponentElement(Type scopeType, Type componentType)
        {
            ScopeType = scopeType;
            ComponentType = componentType;
        }
    }
}