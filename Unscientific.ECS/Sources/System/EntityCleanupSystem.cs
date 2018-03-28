namespace Unscientific.ECS
{
    public abstract class EntityCleanupSystem<TScope, TComponent>: ICleanupSystem where TScope : IScope
    {
        private readonly Context<TScope> _context;

        protected EntityCleanupSystem(Contexts contexts)
        {
            _context = contexts.Get<TScope>();
        }

        public void Cleanup()
        {
            foreach (var entity in _context.AllWith<TComponent>())
            {
                Cleanup(entity);
            }
        }

        protected abstract void Cleanup(Entity<TScope> entity);
    }
    
    public abstract class EntityCleanupSystem<TScope, TComponent1, TComponent2>: ICleanupSystem where TScope : IScope
    {
        private readonly Context<TScope> _context;

        protected EntityCleanupSystem(Contexts contexts)
        {
            _context = contexts.Get<TScope>();
        }

        public void Cleanup()
        {
            foreach (var entity in _context.AllWith<TComponent1, TComponent2>())
            {
                Cleanup(entity);
            }
        }

        protected abstract void Cleanup(Entity<TScope> entity);
    }

    public abstract class EntityCleanupSystem<TScope, TComponent1, TComponent2, TComponent3>: ICleanupSystem where TScope : IScope
    {
        private readonly Context<TScope> _context;

        protected EntityCleanupSystem(Contexts contexts)
        {
            _context = contexts.Get<TScope>();
        }

        public void Cleanup()
        {
            foreach (var entity in _context.AllWith<TComponent1, TComponent2, TComponent3>())
            {
                Cleanup(entity);
            }
        }

        protected abstract void Cleanup(Entity<TScope> entity);
    }
}