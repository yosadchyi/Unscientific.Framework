using System;

namespace Unscientific.ECS
{
    internal class ComponentInfo
    {
        internal int Index;
        internal Type Type;
        internal Func<int, bool> HasComponent;
        internal Func<int, object> GetComponent;
        internal Action<int, object> AddComponent;
        internal Action<int> RemoveComponent;
        internal Action<int, object> ReplaceComponent;
    }
}