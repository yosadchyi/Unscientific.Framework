using System;
using System.Collections.Generic;

namespace Unscientific.ECS.DSL
{
    internal class FeatureElement
    {
        internal readonly List<DependencyElement> Imports;
        internal readonly List<ContextElement> ProvidedContexts;
        internal readonly List<ComponentElement> ProvidedComponents;
        internal readonly List<Action<MessageBus>> MessageCtors;
        internal readonly SystemsElement Systems;

        internal FeatureElement(List<DependencyElement> imports,
            List<ContextElement> providedContexts,
            List<ComponentElement> providedComponents,
            List<Action<MessageBus>> messageCtors,
            SystemsElement systems)
        {
            Imports = imports;
            ProvidedContexts = providedContexts;
            ProvidedComponents = providedComponents;
            MessageCtors = messageCtors;
            Systems = systems;
        }
    }
}