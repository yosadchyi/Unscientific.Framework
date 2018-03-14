using System;
using System.Collections;
using System.Collections.Generic;

namespace Unscientific.ECS
{
    public struct EntityEnumerator<TScope> where TScope : IScope
    {
        private readonly int _count;
        private int _current;

        public EntityEnumerator(int count)
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

        public Entity<TScope> Current {
            get { return new Entity<TScope>(_current); }
        }
    }

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
            while (++_current < _count)
            {
                if (Current.Has<TComponent>())
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            _current = -1;
        }

        public Entity<TScope> Current {
            get
            {
                return new Entity<TScope>(_current);
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
                if (Current.Has<TComponent1>() && Current.Has<TComponent2>())
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            _current = -1;
        }

        public Entity<TScope> Current {
            get { return new Entity<TScope>(_current); }
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
                if (Current.Has<TComponent1>() && Current.Has<TComponent2>() && Current.Has<TComponent3>())
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            _current = -1;
        }

        public Entity<TScope> Current {
            get { return new Entity<TScope>(_current); }
        }
    }

    public struct EntityEnumerable<TScope> where TScope: IScope
    {
    
        private readonly int _count;

        public EntityEnumerable(int count)
        {
            _count = count;
        }

        public EntityEnumerator<TScope> GetEnumerator()
        {
            return new EntityEnumerator<TScope>(_count);
        }

        public Entity<TScope> First()
        {
            var enumerator = GetEnumerator();

            if (enumerator.MoveNext())
                return enumerator.Current;

            throw new NoEntitiesException();
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

    public class Context<TScope> where TScope : IScope
    {
        #region Component Data
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once StaticMemberInGenericType
        // ReSharper disable once CollectionNeverQueried.Global
        internal static readonly List<Type> ComponentTypes = new List<Type>();
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once StaticMemberInGenericType
        internal static event Action<int, int> OnRemove = delegate {  };
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
            // ReSharper disable once StaticMemberInGenericType
            // ReSharper disable once MemberCanBePrivate.Global
            internal static int Id = -1;
            internal static TComponent[] Data;
            // ReSharper disable once StaticMemberInGenericType
            internal static BitArray Present;

            internal static void Init()
            {
                if (Id != -1)
                    return;

                Id = StaticIdAllocator<ComponentData<TComponent>>.AllocateId();
                ComponentTypes.Add(typeof(TComponent));
                OnInit += Init;
                OnRemove += Remove;
                OnGrow += Grow;
                OnClear += Clear;
            }

            private static void Clear()
            {
                Array.Clear(Data, 0, Data.Length);
                Present.SetAll(false);
            }

            private static void Grow(int capacity)
            {
                if (Data.Length < capacity) Array.Resize(ref Data, capacity);
                if (Present.Length < capacity) Present.Length = capacity;
            }

            private static void Init(int capacity)
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                Data = new TComponent[capacity];
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                Present = new BitArray(capacity);
            }

            private static void Remove(int pos, int last)
            {
                Data[pos] = Data[last];
                Present[pos] = Present[last];

                Data[last] = default(TComponent);
                Present[last] = false;
            }
        }
        #endregion

        public class Initializer
        {
            private int _initialCapacity = 128;
            private ReferenceTrackerFactory _referenceTrackerFactory;

            public Initializer()
            {
                ComponentData<Identifier>.Init();
            }

            public Initializer WithReferenceTrackerFactory(ReferenceTrackerFactory referenceTrackerFactory)
            {
                _referenceTrackerFactory = referenceTrackerFactory;
                return this;
            }
            
            public Initializer WithInitialCapacity(int capacity)
            {
                _initialCapacity = capacity;
                return this;
            }

            public Context<TScope> Initialize()
            {
                if (_referenceTrackerFactory == null)
                {
#if UNSAFE_ECS
                    _referenceTrackerFactory = (capacity) => new UnsafeReferenceTracker();
#else
                    _referenceTrackerFactory = (capacity) => new CountingReferenceTracker<TScope>(capacity);
#endif
                }
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                return new Context<TScope>(_initialCapacity, _referenceTrackerFactory);
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

        public Entity<TScope> this[int id]
        {
            get { return GetEntityById(id); }
        }

        /// <summary>
        /// Count of allocated entities/slots in component arrays
        /// </summary>
        private int _count;

        private int _capacity;
        private readonly IReferenceTracker _referenceTracker;
        private readonly Stack<int> _freeList;
        private int[] _id2Index;

        private Context(int initialCapacity, ReferenceTrackerFactory referenceTrackerFactory)
        {
            _capacity = initialCapacity;
            _referenceTracker = referenceTrackerFactory(_capacity);
            _freeList = new Stack<int>(_capacity);
            _id2Index = new int[_capacity];

            for (var i = 0; i < _capacity; i++)
                _id2Index[i] = -1;

            for (var i = _capacity - 1; i >= 0; i--)
                _freeList.Push(i + 1);

            // Initialize scopes
            OnInit(_capacity);

            _count = 0;
            Instance = this;
        }

        public Entity<TScope> GetEntityById(int id)
        {
#if !UNSAFE_ECS
            if (_id2Index[id - 1] == -1)
                throw new EntityDoesNotExistsException<TScope>(id);
#endif
            return new Entity<TScope>(_id2Index[id - 1]);
        }

        public Entity<TScope> CreateEntity()
        {
            if (_count == _capacity)
                Grow(_capacity * 2);

            var index = _count;
            _count++;
            var id = _freeList.Pop();

            var entity = new Entity<TScope>(index).Add(new Identifier(id));

            _id2Index[id - 1] = index;

            return entity;
        }

        internal void Retain(Entity<TScope> entity)
        {
            _referenceTracker.Retain(entity.Id);
        }

        internal void Release(Entity<TScope> entity)
        {
            _referenceTracker.Release(entity.Id);
        }

        public void DestroyEntity(Entity<TScope> entity)
        {
#if !UNSAFE_ECS
            if (_referenceTracker.RetainCount(entity.Id) > 0)
                throw new TryingToDestroyReferencedEntity<TScope>(entity.Id);
#endif
            var index = entity.Index;
            var lastIndex = _count - 1;

#if !UNSAFE_ECS
            _id2Index[entity.Id - 1] = -1;
#endif
            var lastId = Get<Identifier>(lastIndex).Value;

            OnRemove(index, lastIndex);

            _id2Index[lastId - 1] = index;
            _count--;
        }

        public void EnsureCapacity(int capacity)
        {
            if (capacity > _capacity)
                Grow(capacity);
        }

        private void Grow(int newCapacity)
        {
            Array.Resize(ref _id2Index, newCapacity);

#if !UNSAFE_ECS
            for (var i = _capacity; i < newCapacity; i++)
                _id2Index[i] = -1;
#endif
            for (var i = newCapacity - 1; i >= _capacity; i--)
                _freeList.Push(i + 1);

            _referenceTracker.Grow(newCapacity);

            OnGrow(newCapacity);

            _capacity = newCapacity;
        }

        #region Accessing Components
        internal TComponent Get<TComponent>(int index)
        {
#if !UNSAFE_ECS
            EnsureComponentExists<TComponent>(index);
#endif
            return ComponentData<TComponent>.Data[index];
        }

        internal void Add<TComponent>(int index, TComponent component)
        {
#if !UNSAFE_ECS
            if (ComponentData<TComponent>.Present[index])
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new EntityAlreadyHasComponentException<TScope, TComponent>(index);
            }
#endif
            ComponentData<TComponent>.Present[index] = true;
            ComponentData<TComponent>.Data[index] = component;
        }

        internal void Replace<TComponent>(int index, TComponent component)
        {
#if !UNSAFE_ECS
            EnsureComponentExists<TComponent>(index);
#endif
            ComponentData<TComponent>.Data[index] = component;
        }

        internal void Remove<TComponent>(int index)
        {
#if !UNSAFE_ECS
            EnsureComponentExists<TComponent>(index);
#endif
            ComponentData<TComponent>.Present[index] = false;
            ComponentData<TComponent>.Data[index] = default(TComponent);
        }

        internal bool Has<TComponent>(int index)
        {
            return ComponentData<TComponent>.Present[index];
        }

        internal bool Is<TComponent>(int index)
        {
            return ComponentData<TComponent>.Present[index];
        }

        private static void EnsureComponentExists<TComponent>(int index)
        {
            if (!ComponentData<TComponent>.Present[index])
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new EntityHasNoComponentException<TScope, TComponent>(index);
            }
        }
        #endregion

        #region Enumerators
        public Entity<TScope> First()
        {
            return All().First();
        }

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

        public EntityEnumerable<TScope> All()
        {
            return new EntityEnumerable<TScope>(_count);
        }

        public FilteringEntityEnumerable<TScope, TComponent> AllWith<TComponent>()
        {
            return new FilteringEntityEnumerable<TScope, TComponent>(_count);
        }

        public FilteringEntityEnumerable<TScope, TComponent1, TComponent2> AllWith<TComponent1, TComponent2>()
        {
            return new FilteringEntityEnumerable<TScope, TComponent1, TComponent2>(_count);
        }

        public FilteringEntityEnumerable<TScope, TComponent1, TComponent2, TComponent3> AllWith<TComponent1, TComponent2, TComponent3>()
        {
            return new FilteringEntityEnumerable<TScope, TComponent1, TComponent2, TComponent3>(_count);
        }
        #endregion

        public void Clear()
        {
            // clear all components
            OnClear();

            // clear members
            for (var i = 0; i < _capacity; i++)
                _id2Index[i] = -1;

            _freeList.Clear();

            for (var i = _capacity - 1; i >= 0; i--)
                _freeList.Push(i + 1);

            _count = 0;
        }
    }
}