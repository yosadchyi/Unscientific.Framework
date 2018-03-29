using System.Collections.Generic;
using Unscientific.ECS.Modules.Physics.Shapes;

namespace Unscientific.ECS.Modules.Physics
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