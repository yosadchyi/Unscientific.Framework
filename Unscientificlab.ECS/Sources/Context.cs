using System;
using System.Collections;
using System.Collections.Generic;
using Unscientificlab.ECS.Exception;
using Unscientificlab.ECS.ReferenceTracking;
using Unscientificlab.ECS.Util;

namespace Unscientificlab.ECS
{
    internal struct IntToEntityEnumerable<TScope> : IEnumerable<Entity<TScope>> where TScope: IScope
    {
        private struct Enumerator : IEnumerator<Entity<TScope>>
        {
            private readonly IEnumerator<int> _source;

            public Enumerator(IEnumerator<int> source)
            {
                _source = source;
            }

            public void Dispose()
            {
                _source.Dispose();
            }

            public bool MoveNext()
            {
                return _source.MoveNext();
            }

            public void Reset()
            {
                _source.Reset();
            }

            object IEnumerator.Current
            {
                get { return new Entity<TScope>(_source.Current); }
            }

            public Entity<TScope> Current {
                get { return new Entity<TScope>(_source.Current); }
            }
        }
    
        private readonly IEnumerable<int> _source;

        public IntToEntityEnumerable(IEnumerable<int> source)
        {
            _source = source;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Entity<TScope>> GetEnumerator()
        {
            return new Enumerator(_source.GetEnumerator());
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
        internal static readonly List<Action> ShutdownActions = new List<Action>();
    }
    
    // ReSharper disable once UnusedTypeParameter
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ComponentData<TScope, TComponent>
    {
        // ReSharper disable once StaticMemberInGenericType
        internal static readonly int Id;
        internal static TComponent[] Data;
        // ReSharper disable once StaticMemberInGenericType
        internal static BitArray Present;

        static ComponentData()
        {
            Id = StaticIdAllocator<ComponentData<TScope, TComponent>>.AllocateId();
            ScopeData<TScope>.ComponentTypes.Add(typeof(TComponent));
            ScopeData<TScope>.RemoveActions.Add((pos, last) =>
            {
                Data[pos] = Data[last];
                Present[pos] = Present[last];
                Data[last] = default(TComponent);
                Present[last] = false;
            });
            ScopeData<TScope>.InitActions.Add(capacity =>
            {
                if (Data == null)
                {
                    // ReSharper disable once HeapView.ObjectAllocation.Evident
                    Data = new TComponent[capacity];
                }
                else
                {
                    for (var i = 0; i < Data.Length; i++)
                    {
                        Data[i] = default(TComponent);
                    }
                    if (Data.Length < capacity)
                    {
                        Array.Resize(ref Data, capacity);
                    }
                }

                if (Present == null)
                {
                    // ReSharper disable once HeapView.ObjectAllocation.Evident
                    Present = new BitArray(capacity);
                }
                else
                {
                    Present.SetAll(false);
                    if (Present.Length < capacity)
                        Present.Length = capacity;
                }
            });
            ScopeData<TScope>.ExtendActions.Add(capacity =>
            {
                if (Data.Length < capacity)
                    Array.Resize(ref Data, capacity);
                if (Present.Length < capacity)
                    Present.Length = capacity;
            });
            ScopeData<TScope>.ShutdownActions.Add(() =>
            {
                for (var i = 0; i < Data.Length; i++)
                    Data[i] = default(TComponent);
            });
        }
    }

    public class Context<TScope> where TScope : IScope
    {
        public class Initializer
        {
            private int _initialCapacity = 128;
            private ReferenceTrackerFactory _referenceTrackerFactory;
            private int _maxCapacity = int.MaxValue;

            public static Initializer New()
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                return new Initializer();
            }

            public Initializer WithComponent<TComponent>()
            {
                // ReSharper disable once UnusedVariable
                var id = ComponentData<TScope, TComponent>.Id;

                return this;
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

            public Initializer WithReferenceTrackerFactory(ReferenceTrackerFactory referenceTrackerFactory)
            {
                _referenceTrackerFactory = referenceTrackerFactory;
                return this;
            }

            public Context<TScope> Initialize()
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                return new Context<TScope>(_initialCapacity, _maxCapacity, _referenceTrackerFactory);
            }
        }

        public static Context<TScope> Instance { get; private set; }

        public int Capacity
        {
            get { return _capacity; }
        }
        
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Free List containing available entity id's
        /// </summary>
        private readonly Stack<int> _freeList;
        /// <summary>
        /// Count of allocated entities/slots in component arrays
        /// </summary>
        private int _count;
        /// <summary>
        /// Component Array Index to Entity Id mapping
        /// </summary>
        private readonly Dictionary<int, int> _indexToId;
        /// <summary>
        /// Entity Id to Component Array Index mapping
        /// </summary>
        private int[] _entityToIndex;

        private readonly IReferenceTracker _referenceTracker;

        private int _capacity;
        private int _lastUsed;
        private readonly int _maxCapacity;
        
        private Context(int initialCapacity, int maxCapacity, ReferenceTrackerFactory trackerFactory)
        {
            if (Instance != null)
                throw new ContextInitializationException<TScope>();
            
            _capacity = initialCapacity;
            _maxCapacity = maxCapacity;

            // ReSharper disable once HeapView.ObjectAllocation.Evident
            _indexToId = new Dictionary<int, int>(_capacity);

            // ReSharper disable once HeapView.ObjectAllocation.Evident
            _freeList = new Stack<int>(_capacity);

            // Initialize scopes
            foreach (var initAction in ScopeData<TScope>.InitActions)
            {
                initAction(_capacity);
            }

            // ReSharper disable once HeapView.ObjectAllocation.Evident
            _entityToIndex = new int[_capacity];

            for (var i = 0; i < _capacity; i++)
            {
                _entityToIndex[i] = -1;
            }

            _count = 0;

            // ReSharper disable once HeapView.ObjectAllocation.Evident
            _referenceTracker = trackerFactory(_capacity);

            Instance = this;
        }

        public void Shutdown()
        {
            
        }

        public int RetainCount(int id)
        {
            return _referenceTracker.RetainCount(id);
        }

        public Entity<TScope> CreateEntity()
        {
            if (_freeList.Count == 0)
            {
                if (_lastUsed == _capacity)
                {
                    Grow(_capacity * 2);
                }
                _freeList.Push(_lastUsed++);
            }

            var id = _freeList.Pop();

            _entityToIndex[id] = _count;
            _indexToId[_count] = id;
            _count++;

            return new Entity<TScope>(id);
        }

        private void Grow(int newCapacity)
        {
            if (newCapacity > _maxCapacity)
                throw new ContextReachedMaxCapacityException<TScope>();

            foreach (var extend in ScopeData<TScope>.ExtendActions)
            {
                extend(newCapacity);
            }

            _referenceTracker.Grow(newCapacity);
            
            if (_entityToIndex.Length < newCapacity)
                Array.Resize(ref _entityToIndex, newCapacity);

            while (_capacity < newCapacity)
            {
                _entityToIndex[_capacity] = -1;
                _capacity++;
            }
        }

        public void ReleaseEntity(object owner, Entity<TScope> entity)
        {
            var id = entity.Id;
            
            _referenceTracker.Release(owner, id);

            if (_referenceTracker.RetainCount(id) == 0)
                DestroyEntity(entity);
        }

        public void DestroyEntity(Entity<TScope> entity)
        {
            var id = entity.Id;
            
            if (_referenceTracker.RetainCount(id) > 0)
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new ReleasingRetainedEntityException<TScope>(id);
            }

            var lastIndex = _count - 1;
            var lastEntity = _indexToId[lastIndex];
            var index = _entityToIndex[id];

            foreach (var rm in ScopeData<TScope>.RemoveActions)
            {
                rm(index, lastIndex);
            }

            _entityToIndex[lastEntity] = index;
            _entityToIndex[id] = -1;
            _indexToId[index] = lastEntity;
            _indexToId.Remove(lastIndex);
            _count--;
            _freeList.Push(id);
        }

        public Entity<TScope> RetainEntity(object owner, Entity<TScope> entity)
        {
            _referenceTracker.Retain(owner, entity.Id);
            return entity;
        }
        
        public TComponent Get<TComponent>(int index)
        {
            EnsureComponentExists<TComponent>(index);
            return ComponentData<TScope, TComponent>.Data[index];
        }

        public TComponent Get<TComponent>(Entity<TScope> entity)
        {
            return Get<TComponent>(GetIndexById(entity.Id));
        }

        public void Add<TComponent>(int index, TComponent component)
        {
            if (ComponentData<TScope, TComponent>.Present[index])
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new EntityAlreadyHasComponentException<TScope, TComponent>(index);
            }

            ComponentData<TScope, TComponent>.Present[index] = true;
            ComponentData<TScope, TComponent>.Data[index] = component;
        }

        public Entity<TScope> Add<TComponent>(Entity<TScope> entity, TComponent component)
        {
            var index = GetIndexById(entity.Id);

            Add(index, component);
            return entity;
        }

        public void Replace<TComponent>(int index, TComponent component)
        {
            EnsureComponentExists<TComponent>(index);
            ComponentData<TScope, TComponent>.Data[index] = component;
        }

        internal Entity<TScope> Replace<TComponent>(Entity<TScope> entity, TComponent component)
        {
            Replace(GetIndexById(entity.Id), component);
            return entity;
        }

        public void Remove<TComponent>(int index)
        {
            EnsureComponentExists<TComponent>(index);
            ComponentData<TScope, TComponent>.Present[index] = false;
            ComponentData<TScope, TComponent>.Data[index] = default(TComponent);
        }

        internal Entity<TScope> Remove<TComponent>(Entity<TScope> entity)
        {
            Remove<TComponent>(GetIndexById(entity.Id));
            return entity;
        }

        public bool Has<TComponent>(int index)
        {
            return ComponentData<TScope, TComponent>.Present[index];
        }

        public bool Has<TComponent>(Entity<TScope> entity)
        {
            return Has<TComponent>(GetIndexById(entity.Id));
        }

        public bool Is<TComponent>(int index)
        {
            return ComponentData<TScope, TComponent>.Present[index];
        }

        public bool Is<TComponent>(Entity<TScope> entity)
        {
            var index = GetIndexById(entity.Id);

            return Is<TComponent>(index);
        }

        private int GetIndexById(int id)
        {
            var index = _entityToIndex[id];

            if (index < 0)
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new EntityDoesNotExistsException<TScope>(id);
            }
            return index;
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
            return new IntToEntityEnumerable<TScope>(_indexToId.Values);
        }
        
        public void Destroy()
        {
            // TODO
        }
    }
}