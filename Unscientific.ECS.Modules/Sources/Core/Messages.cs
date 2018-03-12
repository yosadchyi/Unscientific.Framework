namespace Unscientific.ECS.Modules.Core
{
    public struct EntityDestroyed<TScope> where TScope : IScope
    {
        private readonly int _entityId;

        public Entity<TScope> Entity
        {
            get { return Context<TScope>.Instance[_entityId]; }
        }

        public EntityDestroyed(Entity<TScope> entity)
        {
            _entityId = entity.Id;
        }
    }
}