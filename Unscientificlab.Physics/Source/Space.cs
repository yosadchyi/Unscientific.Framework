using System;
using System.Collections.Generic;
using Unscientificlab.ECS;
using Unscientificlab.ECS.Base;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.Physics
{
    public class Space
    {
        private readonly ISpatialDatabase _spatialDatabase;

        private int _stamp;

        // Cached delegates
        private readonly SpatialDatabaseCallback _aabbCallback;

        // transient fields
        private SpatialDatabaseCallback _callback;
        private AABB _aabb;
        private readonly SpatialDatabaseCallback _circleCallback;
        private readonly SpatialDatabaseCallback _addToListCallback;
        private Fix _circleRadius;
        private FixVec2 _circlePosition;
        private List<Entity<Simulation>> _list;

        internal ISpatialDatabase SpatialDatabase { get { return _spatialDatabase; } }

        public Space(ISpatialDatabase spatialDatabase)
        {
            _spatialDatabase = spatialDatabase;
            _aabbCallback = AABBCallback;
            _circleCallback = CircleCallback;
            _addToListCallback = AddToList;
        }

        public void QueryCircle(ref FixVec2 position, Fix radius, List<Entity<Simulation>> list)
        {
            _list = list;
            QueryCircle(ref position, radius, _addToListCallback);
            _list = null;
        }

        private void AddToList(Entity<Simulation> entity, Shape shape)
        {
            _list.Add(entity);
        }

        public void QueryCircle(ref FixVec2 position, Fix radius, SpatialDatabaseCallback callback)
        {
            var aabb = new AABB(position, radius);

            _callback = callback;
            _aabb = aabb;
            _circlePosition = position;
            _circleRadius = radius;
            _spatialDatabase.Query(ref aabb, _circleCallback);
            _stamp++;
            _callback = null;
        }

        private void CircleCallback(Entity<Simulation> entity, Shape shape)
        {
            var transform = new Transform(entity.Get<Position>().Value, entity.Get<Orientation>().Value);

            // do not process same shapes during query
            if (shape.Stamp == _stamp)
                return;

            shape.Stamp = _stamp;

            switch (shape.Type)
            {
                case ShapeType.AABB:
                    var aabbShape = (AABBShape) shape;
                    var aabb = aabbShape.GetBoundingBox(ref transform);

                    if (Intersector.AABBIntersectsCircle(ref aabb, ref _circlePosition, _circleRadius))
                        _callback(entity, shape);
                    break;

                case ShapeType.Circle:
                    var circleShape = (CircleShape) shape;

                    if (Intersector.CirclesIntersects(ref _circlePosition, _circleRadius, ref transform.Position, circleShape.Radius))
                        _callback(entity, shape);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void QueryAABB(ref AABB aabb, List<Entity<Simulation>> list)
        {
            _list = list;
            QueryAABB(ref aabb, _addToListCallback);
            _list = null;
        }

        public void QueryAABB(ref AABB aabb, SpatialDatabaseCallback callback)
        {
            _callback = callback;
            _aabb = aabb;
            _spatialDatabase.Query(ref aabb, _aabbCallback);
            _stamp++;
            _callback = null;
        }

        private void AABBCallback(Entity<Simulation> entity, Shape shape)
        {
            var transform = new Transform(entity.Get<Position>().Value, entity.Get<Orientation>().Value);

            // do not process same shapes during query
            if (shape.Stamp == _stamp)
                return;

            shape.Stamp = _stamp;

            switch (shape.Type)
            {
                case ShapeType.Circle:
                    var circle = (CircleShape) shape;

                    if (Intersector.AABBIntersectsCircle(ref _aabb, ref transform.Position, circle.Radius))
                        _callback(entity, shape);
                    break;

                case ShapeType.AABB:
                    var aabb = shape.GetBoundingBox(ref transform);

                    if (_aabb.Intersects(ref aabb))
                        _callback(entity, shape);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}