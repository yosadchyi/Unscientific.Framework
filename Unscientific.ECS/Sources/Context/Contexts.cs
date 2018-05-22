using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Unscientific.ECS
{
    public class Contexts
    {
        private static readonly object[] EmptyObjectsArray = new object[0];

        private readonly List<Type> _registeredContextScopes = new List<Type>();
        private readonly List<CachedMethodInvocation> _clearInvocations = new List<CachedMethodInvocation>();

        internal Contexts(List<ContextInfo> contexts)
        {
            contexts.ForEach(AddContext);
        }

        internal void AddContext(ContextInfo info)
        {
            var contextGenericType = typeof(Context<>);
            var contextType = contextGenericType.MakeGenericType(info.ScopeType);
            var instance = ReflectionUtils.CreateInstance(contextType, info.Components, info.InitialCapacity, info.MaxCapacity);
            var clearMethod = contextType.GetMethod("Clear");

            _clearInvocations.Add(new CachedMethodInvocation(clearMethod, instance, EmptyObjectsArray));
            _registeredContextScopes.Add(info.ScopeType);
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public Context<TScope> Get<TScope>()
        {
            return Context<TScope>.Instance;
        }

        public void Clear()
        {
            foreach (var cachedMethodInvocation in _clearInvocations)
            {
                cachedMethodInvocation.Invoke();
            }
        }
    }
}