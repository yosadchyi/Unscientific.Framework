namespace Unscientific.ECS.Features.Physics2D.Shapes
{
    public abstract class Shape
    {
        public const int NoGroup = 0;
        public const uint AllCategories = uint.MaxValue;

        internal Shape Next;
        internal int Stamp;

        public ShapeType Type;
        public bool Sensor;
        public string Tag;
        public int Group = NoGroup;
        public uint Categories = AllCategories;
        public uint Mask = AllCategories;

        public abstract AABB GetBoundingBox(ref Transform transform);

        public bool IsShapeFilterRejected(Shape other)
        {
            return (Group != 0 && Group == other.Group) ||
                   (Categories & other.Mask) == 0 ||
                   (other.Categories & Mask) == 0;
        }

        protected void Clear()
        {
            Stamp = 0;
            Next = null;
            Tag = null;
            Group = NoGroup;
            Categories = AllCategories;
            Mask = AllCategories;
            Sensor = false;
        }

        public abstract void ReturnToPool();
    }
}