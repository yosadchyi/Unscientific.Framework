using System.Diagnostics.Contracts;

namespace Unscientificlab.ECS
{
    public struct EntityRef<TScope> where TScope : IScope
    {
        public readonly int Id;

        public Entity<TScope> Entity
        {
            get { return Context<TScope>.Instance[Id]; }
        }

        internal EntityRef(int id)
        {
            Id = id;
        }

        public Entity<TScope> Release<TScope1>(Entity<TScope1> owner) where TScope1 : IScope
        {
            Context<TScope>.Instance.Release(Entity);
            return Entity;
        }
    }
    
    public struct Entity<TScope> where TScope: IScope
    {
        internal readonly int Index;

        public int Id
        {
            get { return Get<Identifier>().Value; }
        }

        internal Entity(int index)
        {
            Index = index;
        }

        public EntityRef<TScope> Retain()
        {
            Context<TScope>.Instance.Retain(this);
            return new EntityRef<TScope>(Id);
        }

        [Pure]
        public TComponent Get<TComponent>()
        {
            return Context<TScope>.Instance.Get<TComponent>(Index);
        }

        [Pure]
        public bool Is<TComponent>()
        {
            return Context<TScope>.Instance.Is<TComponent>(Index);
        }

        [Pure]
        public bool Has<TComponent>()
        {
            return Context<TScope>.Instance.Has<TComponent>(Index);
        }

        public Entity<TScope> Add<TComponent>(TComponent component)
        {
            Context<TScope>.Instance.Add(Index, component);
            return this;
        }

        public Entity<TScope> Remove<TComponent>()
        {
            Context<TScope>.Instance.Remove<TComponent>(Index);
            return this;
        }

        public Entity<TScope> Replace<TComponent>(TComponent component)
        {
            Context<TScope>.Instance.Replace(Index, component);
            return this;
        }
    }
}