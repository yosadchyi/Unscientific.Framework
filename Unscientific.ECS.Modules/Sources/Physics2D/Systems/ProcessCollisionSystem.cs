using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D.Shapes;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public class ProcessCollisionSystem: IUpdateSystem
    {
        private readonly MessageBus _messageBus;
        private readonly Context<Singletons> _singletons;

        private Entity<Game> _entity;
        private Shape _shape;
        private AABB _shapeBB = new AABB(0, 0, 0, 0);
        private Transform _transform;
        private int _stamp = 1;
        private readonly SpatialDatabaseCallback _callback;
        private readonly Context<Game> _simulation;

        [SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
        public ProcessCollisionSystem(Contexts contexts, MessageBus messageBus)
        {
            _messageBus = messageBus;
            _callback = CheckCollision;
            _singletons = contexts.Get<Singletons>();
            _simulation = contexts.Get<Game>();
        }

        public void Update()
        {
            var space = _singletons.Singleton().Get<Space>();
            var hash = space.SpatialDatabase;

            foreach (var entity in _simulation.AllWith<Position, BoundingShapes>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                foreach (var shape in entity.Get<BoundingShapes>().Shapes)
                    ProcessShapeCollisions(entity, shape, hash);
            }

            _shape = null;
        }

        private void ProcessShapeCollisions(Entity<Game> entity, Shape shape, ISpatialDatabase hash)
        {
            var collisionsBefore = 0;
            List<Collision> collisions = null;
            _transform = GetEntityTransform(entity);
            _entity = entity;

            if (_entity.Has<Collisions>())
            {
                collisions = _entity.Get<Collisions>().List;
                collisionsBefore = collisions.Count;
                ResetCollisions(collisions);
            }

            _shape = shape;
            _shapeBB = shape.GetBoundingBox(ref _transform);

            hash.Query(ref _shapeBB, _callback);
            hash.Add(entity, shape, ref _shapeBB);

            _stamp++;

            if (collisions != null)
            {
                if (collisionsBefore != 0 || collisions.Count != 0)
                {
                    _entity.Replace(new Collisions(collisions));
                }
            }
        }

        private static void ResetCollisions(List<Collision> collisions)
        {
            foreach (var collision in collisions)
            {
                collision.Return();
            }
            collisions.Clear();
        }

        private static Transform GetEntityTransform(Entity<Game> entity)
        {
            var position = entity.Get<Position>().Value;
            var orientation = entity.Has<Orientation>() ? entity.Get<Orientation>().Value : Fix.Zero;
            var transform = new Transform(position, orientation);
            return transform;
        }

        private void CheckCollision(Entity<Game> entity, Shape shape)
        {
            if (entity.Id == _entity.Id || shape == _shape || shape.IsShapeFilterRejected(shape))
                return;

            if (shape.Stamp == _stamp)
                return;

            var entityTransform = GetEntityTransform(entity);
            var shapeBB = shape.GetBoundingBox(ref entityTransform);

            shape.Stamp = _stamp;

            if (!shapeBB.Intersects(ref _shapeBB))
                return;

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
                if (shape.Type == ShapeType.Circle && _shape.Type == ShapeType.AABB)
                {
                    var circle = (CircleShape) shape;

                    if (Intersector.AABBIntersectsCircle(ref _shapeBB, ref entityTransform.Position, circle.Radius))
                        AddCollisions(entity, shape);
                }
                else if (shape.Type == ShapeType.AABB && _shape.Type == ShapeType.Circle)
                {
                    var circle = (CircleShape) _shape;

                    if (Intersector.AABBIntersectsCircle(ref shapeBB, ref _transform.Position, circle.Radius))
                        AddCollisions(entity, shape);
                }
            }
        }

        private void AddCollisions(Entity<Game> entity, Shape shape)
        {
            _messageBus.Send(new EntitiesCollided(_entity, _shape, entity, shape));
            AddCollision(_entity, _shape, entity, shape);
            AddCollision(entity, shape, _entity, _shape);
        }

        private static void AddCollision(Entity<Game> entity1, Shape shape1, Entity<Game> entity2, Shape shape2)
        {
            if (entity1.Has<Collisions>())
            {
                var collisions = entity1.Get<Collisions>().List;

                collisions.Add(Collision.New(shape1, entity2, shape2));
            }
        }
    }
}