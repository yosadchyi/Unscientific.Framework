using Unscientific.ECS.Features.Physics2D.Shapes;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct BoundingShapes
    {
        public readonly Shape[] Shapes;

        public BoundingShapes(params Shape[] shapes)
        {
            Shapes = shapes;
        }
    }
}