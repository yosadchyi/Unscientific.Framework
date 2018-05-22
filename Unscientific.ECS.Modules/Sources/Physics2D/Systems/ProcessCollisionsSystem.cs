using System;
using System.Collections.Generic;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Destroy;
using Unscientific.ECS.Modules.Physics2D.Shapes;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public static class ProcessCollisionsSystem
    {
        // move to singletons?
        private static Entity<Game> _entity;
        private static Shape _shape;
        private static AABB _shapeBB = new AABB(0, 0, 0, 0);
        private static Transform _transform;
        private static int _stamp = 1;
        private static readonly SpatialDatabaseCallback _callback;
        private static readonly Context<Game> _context;

        public static void Update(Contexts contexts)
        {
            var space = contexts.Singleton().Get<Space>();
            var spatialDatabase = space.SpatialDatabase;

            foreach (var entity in _context.AllWith<BoundingShapes, Position>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                foreach (var shape in entity.Get<BoundingShapes>().Shapes)
                    ProcessShapeCollisions(entity, shape, spatialDatabase);
            }

            _shape = null;
        }

        private static void ProcessShapeCollisions(Entity<Game> entity, Shape shape, ISpatialDatabase spatialDatabase)
        {
            var collisionsBefore = 0;
            List<Collision> collisions = null;
            _transform = GetEntityTransform(entity);
            _entity = entity;

            if (_entity.Has<Collisions>())
            {
                collisions = _entity.Get<Collisions>().List;
                collisionsBefore = collisions.Count;
                collisions.Clear();
            }

            _shape = shape;
            _shapeBB = shape.GetBoundingBox(ref _transform);

            spatialDatabase.Query(ref _shapeBB, _callback);
            spatialDatabase.Add(entity, shape, ref _shapeBB);

            _stamp++;

            if (collisions == null) return;

            if (collisionsBefore != 0 || collisions.Count != 0)
            {
                _entity.Replace(new Collisions(collisions));
            }
        }

        private static Transform GetEntityTransform(Entity<Game> entity)
        {
            var position = entity.Get<Position>().Value;
            var orientation = entity.Has<Orientation>() ? entity.Get<Orientation>().Value : Fix.Zero;
            var transform = new Transform(position, orientation);
            return transform;
        }

        private static void CheckCollision(Entity<Game> entity, Shape shape)
        {
            if (entity.Id == _entity.Id ||
                shape == _shape ||
                shape.IsShapeFilterRejected(shape) ||
                shape.Stamp == _stamp) return;

            var entityTransform = GetEntityTransform(entity);
            var shapeBB = shape.GetBoundingBox(ref entityTransform);

            shape.Stamp = _stamp;

            if (!shapeBB.Intersects(ref _shapeBB)) return;

            if (shape.Type == _shape.Type)
            {
                switch (shape.Type)
                {
                    case ShapeType.AABB:
                        // collision already checked - just generate collisions
                        AddCollisions(entity, shape);
                        break;
                    case ShapeType.Circle:
                        var circle1 = (CircleShape) shape;
                        var circle2 = (CircleShape) _shape;

                        if (Intersector.CirclesIntersects(ref entityTransform.Position, circle1.Radius,
                            ref _transform.Position, circle2.Radius))
                            AddCollisions(entity, shape);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (shape.Type)
                {
                    case ShapeType.Circle when _shape.Type == ShapeType.AABB:
                    {
                        var circle = (CircleShape) shape;

                        if (Intersector.AABBIntersectsCircle(ref _shapeBB, ref entityTransform.Position, circle.Radius))
                            AddCollisions(entity, shape);
                        break;
                    }
                    case ShapeType.AABB when _shape.Type == ShapeType.Circle:
                    {
                        var circle = (CircleShape) _shape;

                        if (Intersector.AABBIntersectsCircle(ref shapeBB, ref _transform.Position, circle.Radius))
                            AddCollisions(entity, shape);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void AddCollisions(Entity<Game> entity, Shape shape)
        {
            // sensors should not collide
            if (_shape.Sensor && shape.Sensor) return;
            AddCollision(_entity, _shape, entity, shape);
            // only sensor should receive collision
            if (!_shape.Sensor) AddCollision(entity, shape, _entity, _shape);
        }

        private static void AddCollision(Entity<Game> entity1, Shape shape1, Entity<Game> entity2, Shape shape2)
        {
            if (!entity1.Has<Collisions>()) return;

            var collisions = entity1.Get<Collisions>().List;

            collisions.Add(new Collision(shape1, entity2, shape2));
        }
    }
}