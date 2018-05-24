using Unscientific.ECS.Features.Physics2D.Shapes;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct BoundingShapes
    {
        public readonly ShapeList Shapes;

        public BoundingShapes(ShapeList shapes)
        {
            Shapes = shapes;
        }
    }
}