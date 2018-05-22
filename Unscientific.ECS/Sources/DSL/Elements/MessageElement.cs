using System;

namespace Unscientific.ECS.DSL
{
    internal class MessageElement
    {
        internal readonly Type MessageType;
        internal readonly int InitialCapacity;
        internal readonly object Aggregator;
        internal readonly bool Delayed;

        internal MessageElement(Type messageType, int initialCapacity, object aggregator, bool delayed)
        {
            MessageType = messageType;
            InitialCapacity = initialCapacity;
            Aggregator = aggregator;
            Delayed = delayed;
        }
    }
}