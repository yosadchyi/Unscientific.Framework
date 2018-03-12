namespace Unscientific.ECS.Modules.Core
{
    public struct EntityDestroyed<TScope> where TScope : IScope
    {
        public readonly EntityRef<TScope> Reference;

        public EntityDestroyed(Entity<TScope> entity)
        {
            Reference = entity.Retain();
        }
    }
}