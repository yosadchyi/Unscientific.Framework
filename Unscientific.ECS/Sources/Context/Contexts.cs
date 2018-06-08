using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Unscientific.ECS
{
    public class Contexts
    {
        private readonly List<IContext> _contexts;

        internal Contexts(List<IContext> contexts)
        {
            _contexts = contexts;
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public Context<TScope> Get<TScope>()
        {
            return Context<TScope>.Instance;
        }

        public void Clear()
        {
            foreach (var context in _contexts)
            {
                context.Clear();
            }
        }
    }
}