using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Unscientific.ECS
{
    public struct FilteringEntityEnumerator<TScope, TComponent> where TScope : IScope
    {
        private readonly int _count;
        private int _current;

        public FilteringEntityEnumerator(int count)
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

        public Entity<TScope> Current
        {
            get
            {
                var id = Context<TScope>.GetEntityId<TComponent>(_current);

                return new Entity<TScope>(id);
            }
        }
    }

    public struct FilteringEntityEnumerator<TScope, TComponent1, TComponent2> where TScope : IScope
    {
        private readonly int _count;
        private int _current;

        public FilteringEntityEnumerator(int count)
        {
            _count = count;
            _current = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            while (++_current < _count)
            {
                if (Current.Has<TComponent2>())
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            _current = -1;
        }

        public Entity<TScope> Current
        {
            get
            {
                var id = Context<TScope>.GetEntityId<TComponent1>(_current);

                return new Entity<TScope>(id);
            }
        }
    }

    public struct FilteringEntityEnumerator<TScope, TComponent1, TComponent2, TComponent3> where TScope : IScope
    {
        private readonly int _count;
        private int _current;

        public FilteringEntityEnumerator(int count)
        {
            _count = count;
            _current = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            while (++_current < _count)
            {
                if (Current.Has<TComponent2>() && Current.Has<TComponent3>())
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            _current = -1;
        }

        public Entity<TScope> Current
        {
            get
            {
                var id = Context<TScope>.GetEntityId<TComponent1>(_current);

                return new Entity<TScope>(id);
            }
        }
    }

    public struct FilteringEntityEnumerable<TScope, TComponent> where TScope: IScope
    {
    
        private readonly int _count;

        public FilteringEntityEnumerable(int count)
        {
            _count = count;
        }

        public FilteringEntityEnumerator<TScope, TComponent> GetEnumerator()
        {
            return new FilteringEntityEnumerator<TScope, TComponent>(_count);
        }
        
        public Entity<TScope> First()
        {
            var enumerator = GetEnumerator();

            if (enumerator.MoveNext())
                return enumerator.Current;

            throw new NoEntitiesException();
        }
    }

    public struct FilteringEntityEnumerable<TScope, TComponent1, TComponent2> where TScope: IScope
    {
    
        private readonly int _count;

        public FilteringEntityEnumerable(int count)
        {
            _count = count;
        }

        public FilteringEntityEnumerator<TScope, TComponent1, TComponent2> GetEnumerator()
        {
            return new FilteringEntityEnumerator<TScope, TComponent1, TComponent2>(_count);
        }
        
        public Entity<TScope> First()
        {
            var enumerator = GetEnumerator();

            if (enumerator.MoveNext())
                return enumerator.Current;

            throw new NoEntitiesException();
        }
    }

    public struct FilteringEntityEnumerable<TScope, TComponent1, TComponent2, TComponent3> where TScope: IScope
    {
    
        private readonly int _count;

        public FilteringEntityEnumerable(int count)
        {
            _count = count;
        }

        public FilteringEntityEnumerator<TScope, TComponent1, TComponent2, TComponent3> GetEnumerator()
        {
            return new FilteringEntityEnumerator<TScope, TComponent1, TComponent2, TComponent3>(_count);
        }
        
        public Entity<TScope> First()
        {
            var enumerator = GetEnumerator();

            if (enumerator.MoveNext())
                return enumerator.Current;

            throw new NoEntitiesException();
        }
    }

    public delegate void ComponentAddedHandler<TScope, in TComponent>(Entity<TScope> entity, TComponent component) where TScope : IScope;
    public delegate void ComponentRemovedHandler<TScope, in TComponent>(Entity<TScope> entity, TComponent component) where TScope : IScope;
    public delegate void ComponentReplacedHandler<TScope, in TComponent>(Entity<TScope> entity, TComponent oldComponent, TComponent newComponent) where TScope : IScope;

    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public class Context<TScope> where TScope : IScope
    {
        #region Component Data
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once StaticMemberInGenericType
        // ReSharper disable once CollectionNeverQueried.Global
        internal static readonly List<Type> ComponentTypes = new List<Type>();
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once StaticMemberInGenericType
        internal static event Action<int> OnRemove = delegate {  };
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once StaticMemberInGenericType
        internal static event Action<int> OnInit = delegate {  };
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once StaticMemberInGenericType
        internal static event Action<int> OnGrow = delegate {  };
        // ReSharper disable once StaticMemberInGenericType
        internal static event Action OnClear = delegate {  };

        // ReSharper disable once ClassNeverInstantiated.Global
        internal class ComponentData<TComponent>
        {
            // ReSharper disable once MemberCanBePrivate.Global
            internal static int Id = -1;
            private static int _capacity;
            internal static int Count;
            internal static TComponent[] Data;
            internal static int[] IndexToEntity;
            internal static int[] EntityToIndex;

            #region Component Event Handlers
            internal static event ComponentAddedHandler<TScope, TComponent> OnComponentAdded; 
            internal static event ComponentRemovedHandler<TScope, TComponent> OnComponentRemoved; 
            internal static event ComponentReplacedHandler<TScope, TComponent> OnComponentReplaced;
            #endregion

            [SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
            internal static void Init()
            {
                if (Id != -1)
                    return;

                Id = StaticIdAllocator<ComponentData<TComponent>>.AllocateId();
                ComponentTypes.Add(typeof(TComponent));
                OnInit += Init;
                OnRemove += id =>
                {
                    if (EntityToIndex[id & IdMask] != -1) DoRemove(id);
                };
                OnGrow += Grow;
                OnClear += Clear;
            }

            private static void Clear()
            {
                Array.Clear(Data, 0, Data.Length);
                ClearEntityToIndex();
                Count = 0;
            }

            private static void ClearEntityToIndex()
            {
                for (var i = 0; i < EntityToIndex.Length; i++)
                {
                    EntityToIndex[i] = -1;
                }
            }

            private static void Grow(int capacity)
            {
                if (_capacity < capacity)
                {
                    Array.Resize(ref Data, capacity);
                    Array.Resize(ref EntityToIndex, capacity);
                    Array.Resize(ref IndexToEntity, capacity);
                    for (var i = _capacity; i < capacity; i++)
                        EntityToIndex[i] = -1;
                    _capacity = capacity;
                }
            }

            private static void DoRemove(int id)
            {
                var entity = id & IdMask;
                var index = EntityToIndex[entity];

                var last = --Count;
                
                Data[index] = Data[last];
                var tmp = IndexToEntity[last];
                IndexToEntity[index] = tmp;
                EntityToIndex[tmp & IdMask] = index;
                EntityToIndex[entity] = -1;

                Data[last] = default(TComponent);
            }

            [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
            private static void Init(int capacity)
            {
                Data = new TComponent[capacity];
                IndexToEntity = new int[capacity];
                EntityToIndex = new int[capacity];
                _capacity = capacity;
                ClearEntityToIndex();
            }

            internal static void Add(int id, ref TComponent component)
            {
                var entity = id & IdMask;
#if !UNSAFE_ECS
                if (EntityToIndex[entity] != -1)
                {
                    // ReSharper disable once HeapView.ObjectAllocation.Evident
                    throw new EntityAlreadyHasComponentException<TScope, TComponent>(id);
                }
#endif
                var index = Count++;

                EntityToIndex[entity] = index;
                IndexToEntity[index] = id;
                Data[index] = component;

                if (OnComponentAdded != null)
                    OnComponentAdded(new Entity<TScope>(id), component);
            }

            internal static void Replace(int id, ref TComponent component)
            {
                var index = EntityToIndex[id & IdMask];

#if !UNSAFE_ECS
                if (index == -1)
                {
                    // ReSharper disable once HeapView.ObjectAllocation.Evident
                    throw new EntityHasNoComponentException<TScope, TComponent>(id);
                }
#endif

                if (OnComponentReplaced != null) {
                    var oldComponent = Data[index];

                    Data[index] = component;

                    OnComponentReplaced(new Entity<TScope>(id), oldComponent, component);
                }
                else
                {
                    Data[index] = component;
                }
            }

            internal static void Remove(int id)
            {
                var index = EntityToIndex[id & IdMask];

#if !UNSAFE_ECS
                if (index == -1)
                {
                    // ReSharper disable once HeapView.ObjectAllocation.Evident
                    throw new EntityHasNoComponentException<TScope, TComponent>(id);
                }
#endif

                if (OnComponentRemoved != null)
                {
                    var component = Data[index];

                    DoRemove(id);
                    OnComponentRemoved(new Entity<TScope>(id), component);
                }
                else
                {
                    DoRemove(id);
                }
            }
        }
        #endregion

        public class Initializer
        {
            private int _initialCapacity = 128;
            private int _maxCapacity = int.MaxValue;

            public Initializer WithInitialCapacity(int capacity)
            {
                _initialCapacity = capacity;
                return this;
            }

            public Initializer WithMaxCapacity(int capacity)
            {
                _maxCapacity = capacity;
                return this;
            }

            public Context<TScope> Initialize()
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                return new Context<TScope>(_initialCapacity, _maxCapacity);
            }
        }

        internal static Context<TScope> Instance { get; private set; }

        public int Capacity => _capacity;

        public int Count => _count;

        public Entity<TScope> this[int id] => GetEntityById(id);

        /// <summary>
        /// Count of allocated entities/slots in component arrays
        /// </summary>
        private int _count;

        private int _capacity;
        private readonly int _maxCapacity;
        private readonly Stack<int> _freeList;
        private ushort[] _generations;

        private const int IdMask = 0xffff;
        private const int GenerationShift = 16;

        private Context(int initialCapacity, int maxCapacity)
        {
            _capacity = initialCapacity;
            _maxCapacity = maxCapacity;
            _freeList = new Stack<int>(_capacity);
            _generations = new ushort[_capacity];

            for (var i = _capacity - 1; i >= 0; i--)
                _freeList.Push(i);

            // Initialize scopes
            OnInit(_capacity);

            _count = 0;
            Instance = this;
        }

        public Entity<TScope> GetEntityById(int id)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(id);
#endif
            return new Entity<TScope>(id);
        }

        public Entity<TScope> CreateEntity()
        {
            if (_count == _capacity)
            {
                if (_capacity == _maxCapacity)
                    throw new ContextReachedMaximalCapacity<TScope>(_maxCapacity);

                Grow(Math.Min(_maxCapacity, _capacity * 2));
            }

            var id = _freeList.Pop();

            _generations[id]++;
            _count++;

            return new Entity<TScope>((_generations[id] << GenerationShift) | id);
        }

        public void DestroyEntity(Entity<TScope> entity)
        {
            var id = entity.Id;
#if !UNSAFE_ECS
            EnsureEntityExists(id);
#endif

            OnRemove(id);
            _freeList.Push(id & IdMask);
            _count--;
        }

        private void Grow(int newCapacity)
        {
            Array.Resize(ref _generations, newCapacity);

            for (var i = newCapacity - 1; i >= _capacity; i--)
                _freeList.Push(i);

            OnGrow(newCapacity);

            _capacity = newCapacity;
        }

        #region Accessing Components
        internal TComponent Get<TComponent>(int id)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(id);
#endif
            id &= IdMask;

            var index = ComponentData<TComponent>.EntityToIndex[id];

            if (index == -1)
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new EntityHasNoComponentException<TScope, TComponent>(id);
            }

            return ComponentData<TComponent>.Data[index];
        }

        internal bool TryGet<TComponent>(int id, out TComponent component)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(id);
#endif
            var index = ComponentData<TComponent>.EntityToIndex[id & IdMask];

            if (index == -1)
            {
                component = default(TComponent);
                return false;
            }
            else
            {
                component = ComponentData<TComponent>.Data[index];
                return true;
            }
        }

        internal void Add<TComponent>(int id, TComponent component)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(id);
#endif
            ComponentData<TComponent>.Add(id, ref component);
        }

        internal void Replace<TComponent>(int id, TComponent component)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(id);
#endif
            ComponentData<TComponent>.Replace(id, ref component);
        }

        internal void Remove<TComponent>(int id)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(id);
#endif
            ComponentData<TComponent>.Remove(id);
        }

        public void RemoveIfExists<TComponent>(int id)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(id);
#endif
            if (ComponentData<TComponent>.EntityToIndex[id & IdMask] != -1)
                ComponentData<TComponent>.Remove(id);
        }

        internal bool Has<TComponent>(int id)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(id);
#endif
            return ComponentData<TComponent>.EntityToIndex[id & IdMask] != -1;
        }

        internal bool Is<TComponent>(int id)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(id);
#endif
            return ComponentData<TComponent>.EntityToIndex[id & IdMask] != -1;
        }

        public bool Exists(int id)
        {
            return _generations[id & IdMask] == ((uint) id >> GenerationShift);
        }

        private void EnsureEntityExists(int id)
        {
            if (!Exists(id))
                throw new EntityDoesNotExistsException<TScope>(id);
        }

        #endregion

        #region Enumerators

        public Entity<TScope> FirstWith<TComponent>()
        {
            return AllWith<TComponent>().First();
        }

        public Entity<TScope> FirstWith<TComponent1, TComponent2>()
        {
            return AllWith<TComponent1, TComponent2>().First();
        }

        public Entity<TScope> FirstWith<TComponent1, TComponent2, TComponent3>()
        {
            return AllWith<TComponent1, TComponent2, TComponent3>().First();
        }

        public FilteringEntityEnumerable<TScope, TComponent> AllWith<TComponent>()
        {
            return new FilteringEntityEnumerable<TScope, TComponent>(ComponentData<TComponent>.Count);
        }

        public FilteringEntityEnumerable<TScope, TComponent1, TComponent2> AllWith<TComponent1, TComponent2>()
        {
            return new FilteringEntityEnumerable<TScope, TComponent1, TComponent2>(ComponentData<TComponent1>.Count);
        }

        public FilteringEntityEnumerable<TScope, TComponent1, TComponent2, TComponent3> AllWith<TComponent1, TComponent2, TComponent3>()
        {
            return new FilteringEntityEnumerable<TScope, TComponent1, TComponent2, TComponent3>(ComponentData<TComponent1>.Count);
        }

        internal static int GetEntityId<TComponent>(int index)
        {
            return ComponentData<TComponent>.IndexToEntity[index];
        }

        #endregion

        public void Clear()
        {
            // clear all components
            OnClear();
            
            // clear members
            Array.Clear(_generations, 0, _capacity);

            _freeList.Clear();
            for (var i = _capacity - 1; i >= 0; i--)
                _freeList.Push(i);

            _count = 0;
        }

        public void AddComponentAddedListener<TComponent>(ComponentAddedHandler<TScope, TComponent> handler)
        {
            ComponentData<TComponent>.OnComponentAdded += handler;
        }

        public void AddComponentRemovedListener<TComponent>(ComponentRemovedHandler<TScope, TComponent> handler)
        {
            ComponentData<TComponent>.OnComponentRemoved += handler;
        }

        public void AddComponentReplacedListener<TComponent>(ComponentReplacedHandler<TScope, TComponent> handler)
        {
            ComponentData<TComponent>.OnComponentReplaced += handler;
        }

        public void RemoveComponentAddedListener<TComponent>(ComponentAddedHandler<TScope, TComponent> handler)
        {
            ComponentData<TComponent>.OnComponentAdded -= handler;
        }

        public void RemoveComponentRemovedListener<TComponent>(ComponentRemovedHandler<TScope, TComponent> handler)
        {
            ComponentData<TComponent>.OnComponentRemoved -= handler;
        }

        public void RemoveComponentReplacedListener<TComponent>(ComponentReplacedHandler<TScope, TComponent> handler)
        {
            ComponentData<TComponent>.OnComponentReplaced -= handler;
        }
    }
}