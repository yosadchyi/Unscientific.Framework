namespace Unscientific.ECS
{
    public abstract class MessageBasedCleanupSystem<TMessage>: ICleanupSystem
    {
        private readonly MessageBus _messageBus;

        protected MessageBasedCleanupSystem(MessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void Cleanup()
        {
            foreach (var message in _messageBus.All<TMessage>())
            {
                ProcessMessage(message);
            }
        }

        protected abstract void ProcessMessage(TMessage message);
    }
}