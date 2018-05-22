using System;
using System.Reflection;

namespace Unscientific.ECS
{
    public static class ReflectionUtils
    {
        public static object CreateInstance(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null, args, null);
        }
    }
}