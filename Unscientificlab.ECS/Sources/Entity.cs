namespace Unscientificlab.ECS
{
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

        public TComponent Get<TComponent>()
        {
            return Context<TScope>.Instance.Get<TComponent>(Index);
        }

        public bool Is<TComponent>()
        {
            return Context<TScope>.Instance.Is<TComponent>(Index);
        }

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