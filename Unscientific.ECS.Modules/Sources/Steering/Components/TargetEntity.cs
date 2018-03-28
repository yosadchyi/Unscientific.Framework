﻿using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Steering
{
    public struct TargetEntity
    {
        public readonly Entity<Game> Entity;

        public TargetEntity(Entity<Game> entityRef)
        {
            Entity = entityRef;
        }
    }
}