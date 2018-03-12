using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics.Shapes;

namespace Unscientific.ECS.Modules.Physics
{
    public delegate void SpatialDatabaseCallback(Entity<Simulation> entity, Shape shape);

    public interface ISpatialDatabase
    {
        void Add(Entity<Simulation> entity, Shape shape, ref AABB aabb);
        void Clear();
        void Remove(Entity<Simulation> entity, ref AABB aabb);
        void Query(ref AABB aabb, SpatialDatabaseCallback callback);
    }
}