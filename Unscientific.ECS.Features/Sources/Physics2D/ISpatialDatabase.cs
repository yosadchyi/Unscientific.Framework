using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D.Shapes;

namespace Unscientific.ECS.Features.Physics2D
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