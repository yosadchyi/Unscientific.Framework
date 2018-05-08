namespace Unscientific.ECS
{
    public struct Entity<TScope> where TScope : IScope
    {
        public readonly int Id;
        public Context<TScope> Context => Context<TScope>.Instance;
        public bool Alive => Context.IsEntityAlive(this);

        private const int IdMask = 0xfffff;
        private const int GenerationShift = 20;

        internal int Index => Id & IdMask;
        internal int Generation => (int) ((uint) Id >> GenerationShift);


        internal Entity(int generation, int index)
        {
            Id = (generation << GenerationShift) | index;
        }

        internal Entity(int id)
        {
            Id = id;
        }

        public TComponent Get<TComponent>()
        {
            return Context.Get<TComponent>(this);
        }

        public bool TryGet<TComponent>(out TComponent component)
        {
            return Context.TryGet(this, out component);
        }

        public bool Is<TComponent>()
        {
            return Context.Is<TComponent>(this);
        }

        public bool Has<TComponent>()
        {
            return Context.Has<TComponent>(this);
        }

        public Entity<TScope> Add<TComponent>(TComponent component)
        {
            Context.Add(this, component);
            return this;
        }

        public Entity<TScope> Remove<TComponent>()
        {
            Context.Remove<TComponent>(this);
            return this;
        }

        public Entity<TScope> RemoveIfExists<TComponent>()
        {
            if (Has<TComponent>()) Remove<TComponent>();
            return this;
        }

        public Entity<TScope> Replace<TComponent>(TComponent component)
        {
            Context.Replace(this, component);
            return this;
        }

        public Entity<TScope> AddOrReplace<TComponent>(TComponent component)
        {
            return Has<TComponent>() ? Replace(component) : Add(component);
        }
    }
}