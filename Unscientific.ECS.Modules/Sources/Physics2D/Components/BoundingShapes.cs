using Unscientific.ECS.Modules.Physics2D.Shapes;

namespace Unscientific.ECS.Modules.Physics2D
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