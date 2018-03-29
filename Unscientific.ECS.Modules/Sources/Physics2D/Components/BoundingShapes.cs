using System.Collections.Generic;
using Unscientific.ECS.Modules.Physics2D.Shapes;

namespace Unscientific.ECS.Modules.Physics2D
{
    public struct BoundingShapes
    {
        public readonly List<Shape> Shapes;

        public BoundingShapes(List<Shape> shapes)
        {
            Shapes = shapes;
        }
    }
}