using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unscientific.Util.Collections;

namespace Unscientific.ECS
{
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public class Context<TScope>
    {
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        internal static event Action<Entity<TScope>> OnRemove = delegate {  };
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        internal static event Action<int> OnInit = delegate {  };
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        internal static event Action<int> OnGrow = delegate {  };
        internal static event Action OnClear = delegate {  };

        private const int NoEntityIndex = -1;

        #region Component Data
        // ReSharper disable once ClassNeverInstantiated.Global
        internal class ComponentData<TComponent>
        {
            // ReSharper disable once MemberCanBePrivate.Global
            internal static int Id = -1;
            private static int _capacity;
            internal static int Count;
            internal static TComponent[] Data;
            internal static int[] ComponentIndexToEntityId;
            internal static int[] EntityIndexToComponentIndex;

            private const int NoComponentIndex = -1;

            #region Component Event Handlers
            internal static event ComponentAddedHandler<TScope, TComponent> OnComponentAdded; 
            internal static event ComponentRemovedHandler<TScope, TComponent> OnComponentRemoved; 
            internal static event ComponentReplacedHandler<TScope, TComponent> OnComponentReplaced;
            #endregion

            internal static void InstantiateType()
            {
                // do nothing
            }
            
            [SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
            public static ComponentInfo Init()
            {
                Id = StaticIdAllocator<ComponentData<TComponent>>.AllocateId();
                OnInit += Init;
                OnRemove += entity =>
                {
                    if (EntityIndexToComponentIndex[entity.Index] != NoEntityIndex)
                        DoRemove(entity);
                };
                OnGrow += Grow;
                OnClear += Clear;

                return new ComponentInfo
                {
                    Type = typeof(TComponent),
                    HasComponent = id => Instance.Has<TComponent>(new Entity<TScope>(id)),
                    GetComponent = id => Instance.Get<TComponent>(new Entity<TScope>(id)),
                    AddComponent = (id, value) => Instance.Add(new Entity<TScope>(id), (TComponent) value),
                    RemoveComponent = id => Instance.Remove<TComponent>(new Entity<TScope>(id)),
                    ReplaceComponent = (id, value) => Instance.Replace(new Entity<TScope>(id), (TComponent) value)
                };
            }

            private static void Clear()
            {
                Array.Clear(Data, 0, Data.Length);
                ClearEntityToIndex();
                Count = 0;
            }

            private static void ClearEntityToIndex()
            {
                for (var i = 0; i < EntityIndexToComponentIndex.Length; i++)
                {
                    EntityIndexToComponentIndex[i] = NoEntityIndex;
                }
            }

            private static void Grow(int capacity)
            {
                if (_capacity < capacity)
                {
                    Array.Resize(ref Data, capacity);
                    Array.Resize(ref EntityIndexToComponentIndex, capacity);
                    Array.Resize(ref ComponentIndexToEntityId, capacity);
                    for (var i = _capacity; i < capacity; i++)
                        EntityIndexToComponentIndex[i] = NoComponentIndex;
                    _capacity = capacity;
                }
            }

            private static void DoRemove(Entity<TScope> entity)
            {
                var entityIndex = entity.Index;
                var componentIndex = EntityIndexToComponentIndex[entityIndex];
                var lastComponentIndex = --Count;
                var lastEntityId = ComponentIndexToEntityId[lastComponentIndex];
                var lastEntity = new Entity<TScope>(lastEntityId);
                
                Data[componentIndex] = Data[lastComponentIndex];
                ComponentIndexToEntityId[componentIndex] = lastEntityId;
                EntityIndexToComponentIndex[lastEntity.Index] = componentIndex;
                EntityIndexToComponentIndex[entityIndex] = NoComponentIndex;
                Data[lastComponentIndex] = default(TComponent);
            }

            [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
            private static void Init(int capacity)
            {
                Data = new TComponent[capacity];
                ComponentIndexToEntityId = new int[capacity];
                EntityIndexToComponentIndex = new int[capacity];
                _capacity = capacity;
                OnComponentAdded = delegate {  };
                OnComponentRemoved = delegate {  };
                OnComponentReplaced = delegate {  };
                ClearEntityToIndex();
            }

            internal static void Get(Entity<TScope> entity, out TComponent component)
            {
                var entityIndex = entity.Index;
                var componentIndex = EntityIndexToComponentIndex[entityIndex];

                if (componentIndex == -1)
                {
                    // ReSharper disable once HeapView.ObjectAllocation.Evident
                    throw new EntityHasNoComponentException<TScope, TComponent>(entity.Id);
                }
                component = Data[componentIndex];
            }

            internal static bool TryGet(Entity<TScope> entity, out TComponent component)
            {
                var componentIndex = EntityIndexToComponentIndex[entity.Index];

                if (componentIndex == NoComponentIndex)
                {
                    component = default(TComponent);
                    return false;
                }
                component = Data[componentIndex];
                return true;
            }
            
            internal static void Add(Entity<TScope> entity, ref TComponent component)
            {
                var entityId = entity.Id;
                var entityIndex = entity.Index;
#if !UNSAFE_ECS
                if (EntityIndexToComponentIndex[entityIndex] != NoComponentIndex)
                {
                    // ReSharper disable once HeapView.ObjectAllocation.Evident
                    throw new EntityAlreadyHasComponentException<TScope, TComponent>(entityId);
                }
#endif
                var componentIndex = Count++;

                EntityIndexToComponentIndex[entity.Index] = componentIndex;
                ComponentIndexToEntityId[componentIndex] = entityId;
                Data[componentIndex] = component;
                if (OnComponentAdded != null)
                    OnComponentAdded(entity);
            }

            internal static void Replace(Entity<TScope> entity, ref TComponent component)
            {
                var componentIndex = EntityIndexToComponentIndex[entity.Index];

#if !UNSAFE_ECS
                if (componentIndex == NoComponentIndex)
                {
                    // ReSharper disable once HeapView.ObjectAllocation.Evident
                    throw new EntityHasNoComponentException<TScope, TComponent>(entity.Id);
                }
#endif

                if (OnComponentReplaced != null) {
                    var oldComponent = Data[componentIndex];

                    Data[componentIndex] = component;
                    OnComponentReplaced(entity, oldComponent);
                }
                else
                {
                    Data[componentIndex] = component;
                }
            }

            internal static void Remove(Entity<TScope> entity)
            {
                var componentIndex = EntityIndexToComponentIndex[entity.Index];

#if !UNSAFE_ECS
                if (componentIndex == NoComponentIndex)
                {
                    // ReSharper disable once HeapView.ObjectAllocation.Evident
                    throw new EntityHasNoComponentException<TScope, TComponent>(entity.Id);
                }
#endif

                if (OnComponentRemoved != null)
                {
                    var component = Data[componentIndex];

                    DoRemove(entity);
                    OnComponentRemoved(entity, component);
                }
                else
                {
                    DoRemove(entity);
                }
            }

            internal static bool ComponentExistsForEntity(Entity<TScope> entity)
            {
                return EntityIndexToComponentIndex[entity.Index] != NoComponentIndex;
            }
        }
        #endregion

        internal static Context<TScope> Instance { get; private set; }

        // ReSharper disable once HeapView.ObjectAllocation.Evident
        internal ComponentInfo[] ComponentsInfo;

        public int Capacity => _capacity;

        public int Count => _count;

        public Entity<TScope> this[int id] => GetEntityById(id);

        /// <summary>
        /// Count of allocated entities/slots in component arrays
        /// </summary>
        private int _count;

        private int _capacity;
        private readonly int _maxCapacity;
        private readonly Deque<int> _freeList;
        private ushort[] _generations;

        internal Context(List<Type> componentTypes, int initialCapacity, int maxCapacity)
        {
            OnClear = delegate { };
            OnGrow = delegate {  };
            OnInit = delegate {  };
            OnRemove = delegate {  };

            InitComponents(componentTypes);
            
            _capacity = initialCapacity;
            _maxCapacity = maxCapacity;
            _freeList = new Deque<int>(_capacity);
            _generations = new ushort[_capacity];

            for (var i = _capacity - 1; i >= 0; i--)
                _freeList.AddBack(i);

            // Initialize scopes
            OnInit(_capacity);

            _count = 0;
            Instance = this;
        }

        private void InitComponents(List<Type> componentTypes)
        {
            var componentsInfo = new List<ComponentInfo>();

            foreach (var componentType in componentTypes)
            {
                var componentDataGenericType = typeof(ComponentData<>);
                var compomentDataType = componentDataGenericType.MakeGenericType(typeof(TScope), componentType);
                var info = (ComponentInfo) compomentDataType.GetMethod("Init").Invoke(null, null);

                componentsInfo.Add(info);
            }

            ComponentsInfo = componentsInfo.ToArray();
        }

        internal static void InstantiateType()
        {
            // do nothing
        }

        public Entity<TScope> GetEntityById(int id)
        {
            var entity = new Entity<TScope>(id);

#if !UNSAFE_ECS
            EnsureEntityExists(entity);
#endif
            return entity;
        }

        public Entity<TScope> CreateEntity()
        {
            if (_count == _capacity)
            {
                if (_capacity == _maxCapacity)
                    throw new ContextReachedMaximalCapacity<TScope>(_maxCapacity);

                Grow(Math.Min(_maxCapacity, _capacity * 2));
            }

            var index = _freeList.RemoveFront();

            if (_generations[index] == 0) _generations[index]++; // in case if object never created/destroyed, give generation number 1
            _count++;

            return new Entity<TScope>(_generations[index], index);
        }

        public void DestroyEntity(Entity<TScope> entity)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(entity);
#endif
            OnRemove(entity);

            var index = entity.Index;

            _generations[index]++;
            if (_generations[index] == 0) _generations[index]++; // handle overflow

            _freeList.AddBack(index);
            _count--;
        }

        private void Grow(int newCapacity)
        {
            Array.Resize(ref _generations, newCapacity);

            for (var i = newCapacity - 1; i >= _capacity; i--)
                _freeList.AddBack(i);

            OnGrow(newCapacity);

            _capacity = newCapacity;
        }

        #region Accessing Components
        internal TComponent Get<TComponent>(Entity<TScope> entity)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(entity);
#endif
            TComponent component;

            ComponentData<TComponent>.Get(entity, out component);
            return component;
        }

        internal bool TryGet<TComponent>(Entity<TScope> entity, out TComponent component)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(entity);
#endif
            return ComponentData<TComponent>.TryGet(entity, out component);
        }

        internal void Add<TComponent>(Entity<TScope> entity, TComponent component)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(entity);
#endif
            ComponentData<TComponent>.Add(entity, ref component);
        }

        internal void Replace<TComponent>(Entity<TScope> entity, TComponent component)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(entity);
#endif
            ComponentData<TComponent>.Replace(entity, ref component);
        }

        internal void Remove<TComponent>(Entity<TScope> entity)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(entity);
#endif
            ComponentData<TComponent>.Remove(entity);
        }

        internal bool Has<TComponent>(Entity<TScope> entity)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(entity);
#endif
            return ComponentData<TComponent>.ComponentExistsForEntity(entity);
        }

        internal bool Is<TComponent>(Entity<TScope> entity)
        {
#if !UNSAFE_ECS
            EnsureEntityExists(entity);
#endif
            return ComponentData<TComponent>.ComponentExistsForEntity(entity);
        }

        internal bool IsEntityAlive(Entity<TScope> entity)
        {
            return _generations[entity.Index] == entity.Generation;
        }

        internal BoxedEntity Box(Entity<TScope> entity)
        {
            return new BoxedEntity(entity.Id, ComponentsInfo);
        }

        private void EnsureEntityExists(Entity<TScope> entity)
        {
            if (!IsEntityAlive(entity))
                throw new EntityDoesNotExistsException<TScope>(entity.Id);
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

        #endregion

        public void Clear()
        {
            // clear all components
            OnClear();
            
            // clear members
            Array.Clear(_generations, 0, _capacity);

            _freeList.Clear();
            for (var i = _capacity - 1; i >= 0; i--)
                _freeList.AddBack(i);

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