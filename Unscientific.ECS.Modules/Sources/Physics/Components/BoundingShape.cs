using Unscientific.ECS.Modules.Physics.Shapes;

namespace Unscientific.ECS.Modules.Physics
{
    public struct BoundingShape
    {
        public readonly Shape Shape;

        public BoundingShape(Shape shape)
        {
            Shape = shape;
        }
    }
}