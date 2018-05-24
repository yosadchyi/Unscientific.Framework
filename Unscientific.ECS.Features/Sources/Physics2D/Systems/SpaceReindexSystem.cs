using System;
using System.Collections.Generic;
using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.ECS.Features.Physics2D.Shapes;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public static class SpaceReindexSystem
    {
        public static void Update(Contexts contexts)
        {
            var space = contexts.Singleton().Get<Space>();
            var spatialDatabase = space.SpatialDatabase;
            var context = contexts.Get<Game>();

            foreach (var entity in context.AllWith<BoundingShapes, Position>())
            {
                if (entity.Is<Destroyed>()) continue;

                var shapes = entity.Get<BoundingShapes>().Shapes;
                var position = entity.Get<Position>().Value;
                var transform = new Transform(position, Fix.Zero);

                for (var shape = shapes.First; shape != null; shape = shape.Next)
                {
                    if (shape.Sensor || !shape.Enabled) continue;
                    
                    var bb = shape.GetBoundingBox(ref transform);
                    
                    spatialDatabase.Add(entity, shape, ref bb);
                }
            }
        }
    }
}