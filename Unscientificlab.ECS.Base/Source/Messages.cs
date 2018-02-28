namespace Unscientificlab.ECS.Base
{
    public struct EntityDestroyed<TScope> where TScope : IScope
    {
        public EntityRef<TScope> Reference { get; }

        public EntityDestroyed(Entity<TScope> entity)
        {
            Reference = entity.Retain(typeof(EntityDestroyed<TScope>));
        }
    }
}