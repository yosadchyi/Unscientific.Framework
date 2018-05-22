namespace Unscientific.ECS.Modules.Core
{
    // ReSharper disable once UnusedTypeParameter
    public struct ComponentAdded<TScope, TComponent>
    {
        public readonly Entity<TScope> Entity;

        public ComponentAdded(Entity<TScope> entity)
        {
            Entity = entity;
        }
    }
}