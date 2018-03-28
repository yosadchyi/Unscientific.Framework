using System;
using System.Collections.Generic;

namespace Unscientific.ECS
{
    public class ContextRegistrations
    {
        private event Action OnRegister = delegate { };

        public ContextRegistrations Add<TScope>(int initialCapacity, int maxCapacity) where TScope : IScope
        {
            OnRegister += () => RegisterContext<TScope>(initialCapacity, maxCapacity);
            return this;
        }

        private static void RegisterContext<TScope>(int initialCapacity, int maxCapacity) where TScope : IScope
        {
            if (Contexts.RegisteredContexts.Contains(typeof(TScope)))
                return;

            var context = new Context<TScope>.Initializer()
                .WithInitialCapacity(initialCapacity)
                .WithMaxCapacity(maxCapacity)
                .Initialize();
            Contexts.OnClear += context.Clear;
            Contexts.RegisteredContexts.Add(typeof(TScope));
        }

        public void Register()
        {
            OnRegister();
        }
    }

    public class Contexts
    {
        internal static readonly List<Type> RegisteredContexts = new List<Type>();
        internal static event Action OnClear = delegate { };

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public Context<TScope> Get<TScope>() where TScope: IScope
        {
            return Context<TScope>.Instance;
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Clear()
        {
            OnClear();
        }
    }
}