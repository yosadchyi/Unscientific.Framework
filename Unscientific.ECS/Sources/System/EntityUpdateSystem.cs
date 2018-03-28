namespace Unscientific.ECS
{
    public abstract class EntityUpdateSystem<TScope, TComponent>: IUpdateSystem where TScope : IScope
    {
        private readonly Context<TScope> _context;

        protected EntityUpdateSystem(Contexts contexts)
        {
            _context = contexts.Get<TScope>();
        }

        public void Update()
        {
            foreach (var entity in _context.AllWith<TComponent>())
            {
                Update(entity);
            }
        }

        protected abstract void Update(Entity<TScope> entity);
    }
    
    public abstract class EntityUpdateSystem<TScope, TComponent1, TComponent2>: IUpdateSystem where TScope : IScope
    {
        private readonly Context<TScope> _context;

        protected EntityUpdateSystem(Contexts contexts)
        {
            _context = contexts.Get<TScope>();
        }

        public void Update()
        {
            foreach (var entity in _context.AllWith<TComponent1, TComponent2>())
            {
                Update(entity);
            }
        }

        protected abstract void Update(Entity<TScope> entity);
    }

    public abstract class EntityUpdateSystem<TScope, TComponent1, TComponent2, TComponent3>: IUpdateSystem where TScope : IScope
    {
        private readonly Context<TScope> _context;

        protected EntityUpdateSystem(Contexts contexts)
        {
            _context = contexts.Get<TScope>();
        }

        public void Update()
        {
            foreach (var entity in _context.AllWith<TComponent1, TComponent2, TComponent3>())
            {
                Update(entity);
            }
        }

        protected abstract void Update(Entity<TScope> entity);
    }
}