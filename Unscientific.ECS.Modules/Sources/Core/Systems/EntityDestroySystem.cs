namespace Unscientific.ECS.Modules.Core
{
    public class DestroySystem<TScope>: MessageBasedCleanupSystem<ComponentAdded<TScope, Destroyed>> where TScope : IScope
    {
        private readonly Context<TScope> _context;

        public DestroySystem(Contexts contexts, MessageBus messageBus) : base(messageBus)
        {
            _context = contexts.Get<TScope>();
        }
        
        protected override void ProcessMessage(ComponentAdded<TScope, Destroyed> message)
        {
            _context.DestroyEntity(message.Entity);
        }
    }
}