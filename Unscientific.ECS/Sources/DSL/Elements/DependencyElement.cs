using System;

namespace Unscientific.ECS.DSL
{
    internal class DependencyElement
    {
        internal readonly Type Tag;

        internal DependencyElement(Type tag)
        {
            Tag = tag;
        }
    }
}