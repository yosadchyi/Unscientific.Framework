﻿using Unscientificlab.ECS;
using Unscientificlab.ECS.Base;
using Unscientificlab.Util.Pool;

namespace Unscientificlab.Physics
{
    public struct EntitiesCollided
    {
        public readonly Entity<Simulation> EntityA;
        public readonly Shape ShapeA;
        public readonly Entity<Simulation> EntityB;
        public readonly Shape ShapeB;

        public EntitiesCollided(Entity<Simulation> entityA, Shape shapeA, Entity<Simulation> entityB, Shape shapeB)
        {
            this.EntityA = entityA;
            this.ShapeA = shapeA;
            this.EntityB = entityB;
            this.ShapeB = shapeB;
        }
    }

    public class Collision
    {
        public static ObjectPool<Collision> Pool = new GenericObjectPool<Collision>(512);

        public Shape SelfShape;
        public int Other;
        public Shape OtherShape;

        public static Collision New(Entity<Simulation> self, Shape selfShape, Entity<Simulation> other, Shape otherShape)
        {
            var collision = Pool.Get();
            collision.SelfShape = selfShape;
            collision.Other = other.Id;
            collision.OtherShape = otherShape;
            return collision;
        }

        public void Return()
        {
            SelfShape = null;
            OtherShape = null;
            Pool.Return(this);
        }

    }
}