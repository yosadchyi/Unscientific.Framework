using System;
using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.ECS.Features.Physics2D.Shapes;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public static class FindCollisionsSystem
    {
        private static readonly SpatialDatabaseCallback Callback = CheckCollision;
        private static Entity<Game> _entity;
        private static Shape _shape;
        private static AABB _shapeBB;
        private static Transform _transform;

        public static void Update(Contexts contexts)
        {
            var space = contexts.Singleton().Get<Space>();
            var spatialDatabase = space.SpatialDatabase;
            var context = contexts.Get<Game>();

            foreach (var entity in context.AllWith<Collisions, BoundingShapes, Position>())
            {
                if (entity.Is<Destroyed>()) continue;

                var collisions = entity.Get<Collisions>().List;
                collisions.Clear();

                var shapes = entity.Get<BoundingShapes>().Shapes;

                var position = entity.Get<Position>().Value;
                _transform = new Transform(position, Fix.Zero);
                _entity = entity;

                for (var shape = shapes.First; shape != null; shape = shape.Next)
                {
                    _shapeBB = shape.GetBoundingBox(ref _transform);
                    _shape = shape;
                    spatialDatabase.Query(ref _shapeBB, Callback);
                }
            }

            _shape = null;
        }
        
        private static void CheckCollision(Entity<Game> entity, Shape shape)
        {
            if (entity.Id == _entity.Id || shape == _shape || shape.IsShapeFilterRejected(shape)) return;

            var position = entity.Get<Position>().Value;
            var entityTransform = new Transform(position, Fix.Zero);
            var shapeBB = shape.GetBoundingBox(ref entityTransform);

            if (!shapeBB.Intersects(ref _shapeBB)) return;

            if (shape.Type == _shape.Type)
            {
                CollideShapesOfSameType(entity, shape, ref entityTransform);
            }
            else
            {
                CollideShapesOfDifferentTypes(entity, shape, ref entityTransform, ref shapeBB);
            }
        }

        private static void CollideShapesOfDifferentTypes(Entity<Game> entity, Shape shape, ref Transform entityTransform, ref AABB shapeBB)
        {
            switch (shape.Type)
            {
                case ShapeType.Circle when _shape.Type == ShapeType.AABB:
                {
                    var circle = (CircleShape) shape;

                    if (Intersector.AABBIntersectsCircle(ref _shapeBB, ref entityTransform.Position, circle.Radius))
                        AddCollision(entity, shape);
                    break;
                }
                case ShapeType.AABB when _shape.Type == ShapeType.Circle:
                {
                    var circle = (CircleShape) _shape;

                    if (Intersector.AABBIntersectsCircle(ref shapeBB, ref _transform.Position, circle.Radius))
                        AddCollision(entity, shape);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void CollideShapesOfSameType(Entity<Game> entity, Shape shape, ref Transform entityTransform)
        {
            switch (shape.Type)
            {
                case ShapeType.AABB:
                    // collision already checked - just generate collisions
                    AddCollision(entity, shape);
                    break;
                case ShapeType.Circle:
                    var circle1 = (CircleShape) shape;
                    var circle2 = (CircleShape) _shape;

                    if (Intersector.CirclesIntersects(ref entityTransform.Position, circle1.Radius,
                        ref _transform.Position, circle2.Radius))
                        AddCollision(entity, shape);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void AddCollision(Entity<Game> entity, Shape shape)
        {
            if (shape.Sensor) return;

            var collisions = _entity.Get<Collisions>().List;

            collisions.Add(new Collision(_shape, entity, shape));
        }
    }
}