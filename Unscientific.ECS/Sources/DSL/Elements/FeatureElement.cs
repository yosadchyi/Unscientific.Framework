using System.Collections.Generic;

namespace Unscientific.ECS.DSL
{
    internal class FeatureElement
    {
        internal readonly List<DependencyElement> Imports;
        internal readonly List<ContextElement> ProvidedContexts;
        internal readonly List<ComponentElement> ProvidedComponents;
        internal readonly List<MessageElement> ProducedMessages;
        internal readonly SystemsElement Systems;

        internal FeatureElement(List<DependencyElement> imports,
            List<ContextElement> providedContexts,
            List<ComponentElement> providedComponents,
            List<MessageElement> producedMessages,
            SystemsElement systems)
        {
            Imports = imports;
            ProvidedContexts = providedContexts;
            ProvidedComponents = providedComponents;
            ProducedMessages = producedMessages;
            Systems = systems;
        }
    }
}