using System;

namespace Unscientific.ECS.DSL
{
    internal class ComponentElement
    {
        internal readonly Type ScopeType;
        internal readonly Func<ComponentInfo> ComponentCtor;

        internal ComponentElement(Type scopeType, Func<ComponentInfo> componentCtor)
        {
            ScopeType = scopeType;
            ComponentCtor = componentCtor;
        }
    }
}