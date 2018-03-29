namespace Unscientific.ECS.Modules.Physics.Shapes
{
    public abstract class Shape
    {
        public const int NO_GROUP = 0;
        public const uint ALL_CATEGORIES = uint.MaxValue;

        public bool Sensor;
        public int Group = NO_GROUP;
        public uint Categories = ALL_CATEGORIES;
        public uint Mask = ALL_CATEGORIES;
        internal int Stamp;

        public abstract ShapeType Type { get; }

        public abstract AABB GetBoundingBox(ref Transform transform);

        public bool IsShapeFilterRejected(Shape other)
        {
            return (Group != 0 && Group == other.Group) ||
                   (Categories & other.Mask) == 0 ||
                   (other.Categories & Mask) == 0;
        }
    }
}