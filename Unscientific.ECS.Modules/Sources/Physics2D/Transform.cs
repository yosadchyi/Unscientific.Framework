using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public struct Transform
    {
        public FixVec2 Position;
        public Fix Rotation;

        public Transform(FixVec2 position, Fix rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public void TransformPoint(ref FixVec2 p)
        {
            p += Position;
        }

        public void TransformAABB(ref AABB aabb)
        {
            aabb.L += Position.X;
            aabb.B += Position.Y;
            aabb.R += Position.X;
            aabb.T += Position.Y;
        }
    }
}