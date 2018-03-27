namespace Unscientific.ECS
{
    public struct EntityRef<TScope> where TScope : IScope
    {
        public readonly int Id;

        public Entity<TScope> Entity => Context<TScope>.Instance[Id];

        internal EntityRef(int id)
        {
            Id = id;
        }

        public Entity<TScope> Release()
        {
            Context<TScope>.Instance.Release(Entity);
            return Entity;
        }
    }
    
    public struct Entity<TScope> where TScope: IScope
    {
        internal readonly int Index;

        public int Id => Get<Identifier>().Value;

        public Context<TScope> Context => Context<TScope>.Instance;

        internal Entity(int index)
        {
            Index = index;
        }

        public EntityRef<TScope> Retain()
        {
            Context.Retain(this);
            return new EntityRef<TScope>(Id);
        }

        public TComponent Get<TComponent>()
        {
            return Context.Get<TComponent>(Index);
        }

        public bool TryGet<TComponent>(out TComponent component)
        {
            return Context.TryGet<TComponent>(Index, out component);
        }

        public bool Is<TComponent>()
        {
            return Context.Is<TComponent>(Index);
        }

        public bool Has<TComponent>()
        {
            return Context.Has<TComponent>(Index);
        }

        public Entity<TScope> Add<TComponent>(TComponent component)
        {
            Context.Add(Index, component);
            return this;
        }

        public Entity<TScope> Remove<TComponent>()
        {
            Context.Remove<TComponent>(Index);
            return this;
        }

        public Entity<TScope> RemoveIfExists<TComponent>()
        {
            Context.RemoveIfExists<TComponent>(Index);
            return this;
        }

        public Entity<TScope> Replace<TComponent>(TComponent component)
        {
            Context.Replace(Index, component);
            return this;
        }
    }
}