namespace Unscientific.ECS
{    
    public struct Entity<TScope> where TScope: IScope
    {
        public readonly int Id;

        public Context<TScope> Context => Context<TScope>.Instance;

        internal Entity(int id)
        {
            Id = id;
        }

        public TComponent Get<TComponent>()
        {
            return Context.Get<TComponent>(Id);
        }

        public bool TryGet<TComponent>(out TComponent component)
        {
            return Context.TryGet(Id, out component);
        }

        public bool Is<TComponent>()
        {
            return Context.Is<TComponent>(Id);
        }

        public bool Has<TComponent>()
        {
            return Context.Has<TComponent>(Id);
        }

        public Entity<TScope> Add<TComponent>(TComponent component)
        {
            Context.Add(Id, component);
            return this;
        }

        public Entity<TScope> Remove<TComponent>()
        {
            Context.Remove<TComponent>(Id);
            return this;
        }

        public Entity<TScope> RemoveIfExists<TComponent>()
        {
            Context.RemoveIfExists<TComponent>(Id);
            return this;
        }

        public Entity<TScope> Replace<TComponent>(TComponent component)
        {
            Context.Replace(Id, component);
            return this;
        }

        public bool Exists()
        {
            return Context.Exists(Id);
        }
    }
}