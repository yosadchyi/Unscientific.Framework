using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Steering2D
{
    public struct TargetEntity
    {
        public readonly Entity<Game> Entity;

        public TargetEntity(Entity<Game> entity)
        {
            Entity = entity;
        }
    }
}