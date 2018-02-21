using System;
using System.Collections;
using System.Collections.Generic;
using Unscientificlab.ECS.Exception;
using Unscientificlab.ECS.ReferenceTracking;
using Unscientificlab.ECS.Util;

namespace Unscientificlab.ECS
{
    internal class DefaultComponentListener<TScope, TComponent>: IComponentListener<TScope, TComponent> where TScope : IScope
    {
        public void OnAdded(Entity<TScope> entity, TComponent component)
        {
            // empty
        }

        public void OnRemoved(Entity<TScope> entity, TComponent component)
        {
            // empty 
        }

        public void OnReplaced(Entity<TScope> entity, TComponent oldComponent, TComponent newComponent)
        {
            // empty 
        }

        public void OnIndexChanged(Entity<TScope> entity, TComponent component)
        {
            // empty 
        }
    }

    internal struct EntityEnumerable<TScope> : IEnumerable<Entity<TScope>> where TScope: IScope
    {
        private struct Enumerator : IEnumerator<Entity<TScope>>
        {
            private readonly int _count;
            private int _current;

            public Enumerator(int count)
            {
                _count = count;
                _current = -1;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return ++_current < _count;
            }

            public void Reset()
            {
                _current = -1;
            }

            object IEnumerator.Current
            {
                get { return new Entity<TScope>(_current); }
            }

            public Entity<TScope> Current {
                get { return new Entity<TScope>(_current); }
            }
        }
    
        private readonly int _count;

        public EntityEnumerable(int count)
        {
            _count = count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Entity<TScope>> GetEnumerator()
        {
            return new Enumerator(_count);
        }
    }

    // ReSharper disable once UnusedTypeParameter
    internal class ScopeData<TScope>
    {
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once StaticMemberInGenericType
        internal static readonly List<Type> ComponentTypes = new List<Type>();
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once StaticMemberInGenericType
        internal static readonly List<Action<int, int>> RemoveActions = new List<Action<int, int>>();
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once StaticMemberInGenericType
        internal static readonly List<Action<int>> InitActions = new List<Action<int>>();
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once StaticMemberInGenericType
        internal static readonly List<Action<int>> ExtendActions = new List<Action<int>>();
        // ReSharper disable once StaticMemberInGenericType
        internal static readonly List<Action> CleanupActions = new List<Action>();

        internal static void Reset()
        {
            ComponentTypes.Clear();
            RemoveActions.Clear();
            InitActions.Clear();
            ExtendActions.Clear();
            CleanupActions.Clear();
        }
    }
    
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ComponentData<TScope, TComponent> where TScope : IScope
    {
        // ReSharper disable once StaticMemberInGenericType
        internal static int Id;
        internal static IComponentListener<TScope, TComponent> Listener;
        internal static TComponent[] Data;
        // ReSharper disable once StaticMemberInGenericType
        internal static BitArray Present;
        private static readonly IComponentListener<TScope, TComponent> defaultComponentListener = new DefaultComponentListener<TScope, TComponent>();

        internal static void Init(IComponentListener<TScope, TComponent> listener)
        {
            Id = StaticIdAllocator<ComponentData<TScope, TComponent>>.AllocateId();
            Listener = listener ?? defaultComponentListener;
            ScopeData<TScope>.ComponentTypes.Add(typeof(TComponent));
            ScopeData<TScope>.RemoveActions.Add((pos, last) =>
            {
                var old = Data[pos];
                var present = Present[pos];

                Data[pos] = Data[last];
                Present[pos] = Present[last];

                Data[last] = default(TComponent);
                Present[last] = false;

                if (pos != last)
                    Listener.OnIndexChanged(new Entity<TScope>(last), Data[last]);

                if (present)
                    Listener.OnRemoved(new Entity<TScope>(pos), old);
            });
            ScopeData<TScope>.InitActions.Add(capacity =>
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                Data = new TComponent[capacity];
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                Present = new BitArray(capacity);
            });
            ScopeData<TScope>.ExtendActions.Add(capacity =>
            {
                if (Data.Length < capacity)
                    Array.Resize(ref Data, capacity);
                if (Present.Length < capacity)
                    Present.Length = capacity;
            });
            ScopeData<TScope>.CleanupActions.Add(() =>
            {
                for (var i = 0; i < Data.Length; i++)
                {
                    Data[i] = default(TComponent);
                    Present[i] = false;
                }
            });
        }
    }

    public class Context<TScope> where TScope : IScope
    {
        public class Initializer
        {
            public class Components
            {
                private readonly Initializer _initializer;

                internal Components(Initializer initializer)
                {
                    _initializer = initializer;
                }

                public Components Add<TComponent>(IComponentListener<TScope, TComponent> listener = null)
                {
                    ComponentData<TScope, TComponent>.Init(listener);
                    return this;
                }

                public Initializer Done()
                {
                    return _initializer;
                }
            }

            private int _initialCapacity = 128;
            private int _maxCapacity = int.MaxValue;

            public Initializer()
            {
                ScopeData<TScope>.Reset();
            }
            
            public Initializer WithComponent<TComponent>(IComponentListener<TScope, TComponent> listener = null)
            {
                // ReSharper disable once UnusedVariable
                ComponentData<TScope, TComponent>.Init(listener);
                return this;
            }

            public Components WithComponents()
            {
                return new Components(this);
            }

            public Initializer WithInitialCapacity(int capacity)
            {
                _initialCapacity = capacity;
                return this;
            }

            public Initializer WithMaxCapacity(int maxCapacity)
            {
                _maxCapacity = maxCapacity;
                return this;
            }

            public Context<TScope> Initialize()
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                return new Context<TScope>(_initialCapacity, _maxCapacity);
            }
        }

        internal static Context<TScope> Instance { get; private set; }

        public int Capacity
        {
            get { return _capacity; }
        }
        
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Count of allocated entities/slots in component arrays
        /// </summary>
        private int _count;

        private int _capacity;
        private readonly int _maxCapacity;
        
        private Context(int initialCapacity, int maxCapacity)
        {
            _capacity = initialCapacity;
            _maxCapacity = maxCapacity;

            // Initialize scopes
            foreach (var initAction in ScopeData<TScope>.InitActions)
                initAction(_capacity);

            _count = 0;
            Instance = this;
        }

        public Entity<TScope> CreateEntity()
        {
            if (_count == _capacity)
                Grow(_capacity * 2);

            var index = _count;
            _count++;
            return new Entity<TScope>(index);
        }

        private void Grow(int newCapacity)
        {
            if (_capacity == _maxCapacity)
                throw new ContextReachedMaxCapacityException<TScope>();

            if (_maxCapacity < newCapacity)
                newCapacity = _maxCapacity;

            foreach (var extend in ScopeData<TScope>.ExtendActions)
                extend(newCapacity);

            _capacity = newCapacity;
        }

        public void DestroyEntity(Entity<TScope> entity)
        {
            var index = entity.Index;            
            var lastIndex = _count - 1;

            foreach (var rm in ScopeData<TScope>.RemoveActions)
                rm(index, lastIndex);

            _count--;
        }

        internal TComponent Get<TComponent>(int index)
        {
            EnsureComponentExists<TComponent>(index);
            return ComponentData<TScope, TComponent>.Data[index];
        }

        internal void Add<TComponent>(int index, TComponent component)
        {
            if (ComponentData<TScope, TComponent>.Present[index])
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new EntityAlreadyHasComponentException<TScope, TComponent>(index);
            }

            ComponentData<TScope, TComponent>.Present[index] = true;
            ComponentData<TScope, TComponent>.Data[index] = component;
            ComponentData<TScope, TComponent>.Listener.OnAdded(new Entity<TScope>(index), component);
        }

        internal void Replace<TComponent>(int index, TComponent component)
        {
            EnsureComponentExists<TComponent>(index);

            var old = ComponentData<TScope, TComponent>.Data[index];
            ComponentData<TScope, TComponent>.Data[index] = component;
            ComponentData<TScope, TComponent>.Listener.OnReplaced(new Entity<TScope>(index), old, component);
        }

        internal void Remove<TComponent>(int index)
        {
            EnsureComponentExists<TComponent>(index);
            var old = ComponentData<TScope, TComponent>.Data[index];
            ComponentData<TScope, TComponent>.Present[index] = false;
            ComponentData<TScope, TComponent>.Data[index] = default(TComponent);
            ComponentData<TScope, TComponent>.Listener.OnRemoved(new Entity<TScope>(index), old);
        }

        internal bool Has<TComponent>(int index)
        {
            return ComponentData<TScope, TComponent>.Present[index];
        }

        internal bool Is<TComponent>(int index)
        {
            return ComponentData<TScope, TComponent>.Present[index];
        }

        private static void EnsureComponentExists<TComponent>(int index)
        {
            if (!ComponentData<TScope, TComponent>.Present[index])
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new EntityHasNoComponentException<TScope, TComponent>(index);
            }
        }

        public IEnumerable<Entity<TScope>> All()
        {
            return new EntityEnumerable<TScope>(_count);
        }

        public void Cleanup()
        {
            foreach (var cleanup in ScopeData<TScope>.CleanupActions)
                cleanup();

            _count = 0;
        }
    }
}