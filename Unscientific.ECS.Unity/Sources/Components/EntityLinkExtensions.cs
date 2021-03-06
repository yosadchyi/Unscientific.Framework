﻿using UnityEngine;
using Unscientific.ECS.Features.Core;

namespace Unscientific.ECS.Unity
{
    public static class EntityLinkExtensions
    {
        public static void LinkToEntity<TScope>(this GameObject gameObject, Entity<TScope> entity)
        {
            var entityLink = gameObject.GetComponent<EntityLink<TScope>>();

            if (entityLink != null)
            {
                entityLink.Entity = entity;
                entityLink.EntityId = entity.Id;
            }
        }
        
        public static void UnlinkEntity(this GameObject gameObject)
        {
            gameObject.UnlinkEntity<Game>();
        }
        
        public static void UnlinkEntity<TScope>(this GameObject gameObject)
        {
            var entityLink = gameObject.GetComponent<EntityLink<TScope>>();

            if (entityLink != null)
            {
                entityLink.EntityId = 0;
            }
        }

    }
}