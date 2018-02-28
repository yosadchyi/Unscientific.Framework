namespace Unscientificlab.ECS.Base
{
    public class DestroySystem<TScope>: ICleanupSystem where TScope : IScope
    {
        private readonly MessageBus _messageBus;

        public DestroySystem(Contexts contexts, MessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        
        public void Cleanup()
        {
            foreach (var message in _messageBus.All<EntityDestroyed<TScope>>())
            {
                var entity = message.Reference.Release(typeof(EntityDestroyed<TScope>));
                
                entity.Destroy();
            }
        }
    }
}