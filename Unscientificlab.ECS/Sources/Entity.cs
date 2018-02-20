namespace Unscientificlab.ECS
{
    public struct Entity<TScope> where TScope: IScope
    {
        public readonly int Id;

        internal Entity(int id)
        {
            Id = id;
        }

        public Entity<TScope> Retain(object owner)
        {
            return Context<TScope>.Instance.RetainEntity(owner, this);
        }

        public void Release(object owner)
        {
            Context<TScope>.Instance.ReleaseEntity(owner, this);
        }

        public TComponent Get<TComponent>()
        {
            return Context<TScope>.Instance.Get<TComponent>(this);
        }
        
        public bool Is<TComponent>()
        {
            return Context<TScope>.Instance.Is<TComponent>(this);
        }

        public bool Has<TComponent>()
        {
            return Context<TScope>.Instance.Has<TComponent>(this);
        }

        public Entity<TScope> Add<TComponent>(TComponent component)
        {
            return Context<TScope>.Instance.Add(this, component);
        }

        public Entity<TScope> Remove<TComponent>()
        {
            return Context<TScope>.Instance.Remove<TComponent>(this);
        }
        
        public Entity<TScope> Replace<TComponent>(TComponent component)
        {
            return Context<TScope>.Instance.Replace(this, component);
        }
    }
}