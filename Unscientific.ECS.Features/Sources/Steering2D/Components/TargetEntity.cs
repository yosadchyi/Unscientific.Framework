using Unscientific.ECS.Features.Core;

namespace Unscientific.ECS.Features.Steering2D
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