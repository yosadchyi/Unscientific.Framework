namespace Unscientific.ECS.Features.Core
{
    public static class MessageBusExtensions
    {
        public static MessageEnumerable<ComponentAdded<TScope, TComponent>> AllComponentAdded<TScope, TComponent>(this MessageBus bus)
        {
            return bus.All<ComponentAdded<TScope, TComponent>>();
        } 
        
        public static MessageEnumerable<ComponentRemoved<TScope, TComponent>> AllComponentRemoved<TScope, TComponent>(this MessageBus bus)
        {
            return bus.All<ComponentRemoved<TScope, TComponent>>();
        } 
        
        public static MessageEnumerable<ComponentReplaced<TScope, TComponent>> AllComponentReplaced<TScope, TComponent>(this MessageBus bus)
        {
            return bus.All<ComponentReplaced<TScope, TComponent>>();
        }
    }
}