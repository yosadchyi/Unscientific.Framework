﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unscientificlab.ECS.Exception;
using Unscientificlab.ECS.ReferenceTracking;
using Unscientificlab.ECS.Util;

namespace Unscientificlab.ECS
{
    internal struct EntityEnumerator<TScope> : IEnumerator<Entity<TScope>> where TScope : IScope
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

        object IEnumerator.Current
        {
            get { return new Entity<TScope>(_current); }
        }

        public Entity<TScope> Current {
            get { return new Entity<TScope>(_current); }
        }
    }

    internal struct FilteringEntityEnumerator<TScope, TComponent> : IEnumerator<Entity<TScope>> where TScope : IScope
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

        object IEnumerator.Current
        {
            get { return new Entity<TScope>(_current); }
        }

        public Entity<TScope> Current {
            get { return new Entity<TScope>(_current); }
        }
    }
    
    internal struct FilteringEntityEnumerator<TScope, TComponent1, TComponent2> : IEnumerator<Entity<TScope>> where TScope : IScope
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

        object IEnumerator.Current
        {
            get { return new Entity<TScope>(_current); }
        }

        public Entity<TScope> Current {
            get { return new Entity<TScope>(_current); }
        }
    }


    internal struct EntityEnumerable<TScope> : IEnumerable<Entity<TScope>> where TScope: IScope
    {
    
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
            return new EntityEnumerator<TScope>(_count);
        }
    }

    internal struct FilteringEntityEnumerable<TScope, TComponent> : IEnumerable<Entity<TScope>> where TScope: IScope
    {
    
        private readonly int _count;

        public FilteringEntityEnumerable(int count)
        {
            _count = count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Entity<TScope>> GetEnumerator()
        {
            return new FilteringEntityEnumerator<TScope, TComponent>(_count);
        }
    }

    internal struct FilteringEntityEnumerable<TScope, TComponent1, TComponent2> : IEnumerable<Entity<TScope>> where TScope: IScope
    {
    
        private readonly int _count;

        public FilteringEntityEnumerable(int count)
        {
            _count = count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Entity<TScope>> GetEnumerator()
        {
            return new FilteringEntityEnumerator<TScope, TComponent1, TComponent2>(_count);
        }
    }

    // ReSharper disable once UnusedTypeParameter
    internal static class ScopeData<TScope>
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
        internal static TComponent[] Data;
        // ReSharper disable once StaticMemberInGenericType
        internal static BitArray Present;

        internal static void Init()
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
                Array.Clear(Data, 0, Data.Length);
                Present.SetAll(false);
            });
        }
    }

    public class Context<TScope> where TScope : IScope
    {
        public class Initializer
        {
            public class ComponentsInitializer
            {
                private readonly Initializer _initializer;

                internal ComponentsInitializer(Initializer initializer)
                {
                    _initializer = initializer;
                }

                public ComponentsInitializer Add<TComponent>()
                {
                    ComponentData<TScope, TComponent>.Init();
                    return this;
                }

                public Initializer Done()
                {
                    return _initializer;
                }
            }

            private int _initialCapacity = 128;
            private int _maxCapacity = int.MaxValue;
            private IReferenceTracker _referenceTracker;

            public Initializer()
            {
                ScopeData<TScope>.Reset();
                ComponentData<TScope, Identifier>.Init();
            }

            public ComponentsInitializer WithComponents()
            {
                return new ComponentsInitializer(this);
            }

            public Initializer WithReferenceTracker(IReferenceTracker referenceTracker)
            {
                _referenceTracker = referenceTracker;
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

            public Context<TScope> Initialize()
            {
                if (_referenceTracker == null)
                {
                    _referenceTracker = new SafeReferenceTracker<TScope>(_initialCapacity);
                }
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                return new Context<TScope>(_initialCapacity, _maxCapacity, _referenceTracker);
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
        private readonly int _maxCapacity;
        private readonly IReferenceTracker _referenceTracker;
        private readonly Stack<int> _freeList;
        private int[] _id2Index;

        private Context(int initialCapacity, int maxCapacity, IReferenceTracker referenceTracker)
        {
            _capacity = initialCapacity;
            _maxCapacity = maxCapacity;
            _referenceTracker = referenceTracker;
            _freeList = new Stack<int>(_capacity);
            _id2Index = new int[_capacity];

            for (var i = 0; i < _capacity; i++)
                _id2Index[i] = -1;

            for (var i = _capacity - 1; i >= 0; i--)
                _freeList.Push(i + 1);

            // Initialize scopes
            foreach (var initAction in ScopeData<TScope>.InitActions)
                initAction(_capacity);

            _count = 0;
            Instance = this;
        }

        public void Retain(Entity<TScope> entity, object owner)
        {
            _referenceTracker.Retain(entity.Id, owner);
        }

        public void Release(Entity<TScope> entity, object owner)
        {
            _referenceTracker.Release(entity.Id, owner);
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

        private void Grow(int newCapacity)
        {
            if (_capacity == _maxCapacity)
                throw new ContextReachedMaxCapacityException<TScope>();

            if (_maxCapacity < newCapacity)
                newCapacity = _maxCapacity;

            Array.Resize(ref _id2Index, newCapacity);

            for (var i = _capacity; i < newCapacity; i++)
                _id2Index[i] = -1;

            for (var i = newCapacity - 1; i >= _capacity; i--)
                _freeList.Push(i + 1);
            
            _referenceTracker.Grow(newCapacity);

            foreach (var extend in ScopeData<TScope>.ExtendActions)
                extend(newCapacity);

            _capacity = newCapacity;
        }

        public void DestroyEntity(Entity<TScope> entity)
        {
            if (_referenceTracker.RetainCount(entity.Id) > 0)
                throw new TryingToDestroyReferencedEntity<TScope>(entity.Id);

            var index = entity.Index;
            var lastIndex = _count - 1;

            _id2Index[entity.Id - 1] = -1;

            if (lastIndex >= 0)
            {
                var lastId = Get<Identifier>(lastIndex).Value;

                foreach (var rm in ScopeData<TScope>.RemoveActions)
                    rm(index, lastIndex);

                _id2Index[lastId - 1] = index;
            }

            _count--;
        }

        public Entity<TScope> GetEntityById(int id)
        {
            if (_id2Index[id - 1] == -1)
                throw new EntityDoesNotExistsException<TScope>(id);

            return new Entity<TScope>(_id2Index[id - 1]);
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
        }

        internal void Replace<TComponent>(int index, TComponent component)
        {
            EnsureComponentExists<TComponent>(index);
            ComponentData<TScope, TComponent>.Data[index] = component;
        }

        internal void Remove<TComponent>(int index)
        {
            EnsureComponentExists<TComponent>(index);
            ComponentData<TScope, TComponent>.Present[index] = false;
            ComponentData<TScope, TComponent>.Data[index] = default(TComponent);
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

        public IEnumerable<Entity<TScope>> AllWith<TComponent>()
        {
            return new FilteringEntityEnumerable<TScope, TComponent>(_count);
        }

        public IEnumerable<Entity<TScope>> AllWith<TComponent1, TComponent2>()
        {
            return new FilteringEntityEnumerable<TScope, TComponent1, TComponent2>(_count);
        }

        public void Cleanup()
        {
            foreach (var cleanup in ScopeData<TScope>.CleanupActions)
                cleanup();

            _count = 0;
        }
    }
}