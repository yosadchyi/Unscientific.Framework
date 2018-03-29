using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D.Shapes;

namespace Unscientific.ECS.Modules.Physics2D
{
    public delegate void SpatialDatabaseCallback(Entity<Game> entity, Shape shape);

    public interface ISpatialDatabase
    {
        void Add(Entity<Game> entity, Shape shape, ref AABB aabb);
        void Clear();
        void Remove(Entity<Game> entity, ref AABB aabb);
        void Query(ref AABB aabb, SpatialDatabaseCallback callback);
    }
}