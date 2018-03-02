namespace Unscientificlab.ECS.Modules.Base
{
    public struct EntityDestroyed<TScope> where TScope : IScope
    {
        public Entity<TScope> Entity { get; }

        public EntityDestroyed(Entity<TScope> entity)
        {
            Entity = entity;
        }
    }
}