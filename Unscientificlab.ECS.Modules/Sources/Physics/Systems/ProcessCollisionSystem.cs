using System;
using System.Collections.Generic;
using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Physics
{
    public class ProcessCollisionSystem: IUpdateSystem, ICleanupSystem
    {
        private readonly MessageBus _messageBus;
        private readonly Entity<Configuration> _configuration;
        private readonly Entity<Singletons> _singletons;

        private Entity<Simulation> _entity;
        private Shape _shape;
        private AABB _shapeBB = new AABB(0, 0, 0, 0);
        private Transform _transform;
        private int _stamp = 1;
        private readonly SpatialDatabaseCallback _callback;
        private readonly Context<Simulation> _simulation;

        public ProcessCollisionSystem(Contexts contexts, MessageBus messageBus)
        {
            _messageBus = messageBus;
            _callback = CheckCollision;
            _singletons = contexts.Get<Singletons>().First();
            _simulation = contexts.Get<Simulation>();
            _configuration = contexts.Get<Configuration>().First();
        }

        public void Update()
        {
            var space = _singletons.Get<Space>();
            var hash = space.SpatialDatabase;

            foreach (var entity in _simulation.AllWith<Position, BoundingShape>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                var collisionsBefore = 0;
                var shape = entity.Get<BoundingShape>().Shape;
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

            _shape = null;
        }

        public void Cleanup()
        {
            _singletons.Get<Space>().SpatialDatabase.Clear();
        }

        private static void ResetCollisions(List<Collision> collisions)
        {
            foreach (var collision in collisions)
            {
                collision.Return();
            }
            collisions.Clear();
        }

        private static Transform GetEntityTransform(Entity<Simulation> entity)
        {
            var position = entity.Get<Position>().Value;
            var orientation = entity.Has<Orientation>() ? entity.Get<Orientation>().Value : Fix.Zero;
            var transform = new Transform(position, orientation);
            return transform;
        }

        private void CheckCollision(Entity<Simulation> entity, Shape shape)
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

        private void AddCollisions(Entity<Simulation> entity, Shape shape)
        {
            _messageBus.Send(new EntitiesCollided(_entity, _shape, entity, shape));
            AddCollision(_entity, _shape, entity, shape);
            AddCollision(entity, shape, _entity, _shape);
        }

        private static void AddCollision(Entity<Simulation> entity1, Shape shape1, Entity<Simulation> entity2, Shape shape2)
        {
            if (entity1.Has<Collisions>())
            {
                var collisions = entity1.Get<Collisions>().List;

                collisions.Add(Collision.New(shape1, entity2, shape2));
            }
        }
    }
}