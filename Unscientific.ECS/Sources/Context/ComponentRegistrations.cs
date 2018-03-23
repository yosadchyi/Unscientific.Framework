using System;
using System.Collections.Generic;

namespace Unscientific.ECS
{
    public class ComponentRegistrations
    {
        private event Action OnRegister = delegate { };

        public class ScopedRegistrator<TScope> where TScope : IScope
        {
            private readonly ComponentRegistrations _componentRegistrations;
            // ReSharper disable once StaticMemberInGenericType
            private static readonly HashSet<Type> RegisteredComponents = new HashSet<Type>();

            public ScopedRegistrator(ComponentRegistrations componentRegistrations)
            {
                _componentRegistrations = componentRegistrations;
            }

            public ScopedRegistrator<TScope> Add<TComponent>()
            {
                _componentRegistrations.OnRegister += DoRegister<TComponent>;
                return this;
            }

            private static void DoRegister<TComponent>()
            {
                if (RegisteredComponents.Contains(typeof(TComponent)))
                    return;

                Context<TScope>.ComponentData<TComponent>.Init();
                RegisteredComponents.Add(typeof(TComponent));
            }

            public ComponentRegistrations End()
            {
                return _componentRegistrations;
            }
        }

        public ScopedRegistrator<TScope> For<TScope>() where TScope : IScope
        {
            return new ScopedRegistrator<TScope>(this);
        }

        public void Register()
        {
            OnRegister();
        }
    }
}