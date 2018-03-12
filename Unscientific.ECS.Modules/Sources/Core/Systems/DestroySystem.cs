namespace Unscientific.ECS.Modules.Core
{
    public class DestroySystem<TScope>: ICleanupSystem where TScope : IScope
    {
        private readonly Context<TScope> _context;
        private readonly MessageBus _messageBus;

        public DestroySystem(Contexts contexts, MessageBus messageBus)
        {
            _context = contexts.Get<TScope>();
            _messageBus = messageBus;
        }
        
        public void Cleanup()
        {
            foreach (var message in _messageBus.All<EntityDestroyed<TScope>>())
            {
                _context.DestroyEntity(message.Entity);
            }
        }
    }
}