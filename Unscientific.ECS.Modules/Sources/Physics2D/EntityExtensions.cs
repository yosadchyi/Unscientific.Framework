using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public static class EntityExtensions
    {
        public static Fix DistanceSqr<TScope>(this Entity<TScope> self, Entity<TScope> other)
        {
            return (other.Get<Position>().Value - self.Get<Position>().Value).MagnitudeSqr;
        }        
        
        public static Fix Distance<TScope>(this Entity<TScope> self, Entity<TScope> other)
        {
            return (other.Get<Position>().Value - self.Get<Position>().Value).Magnitude;
        }
    }
}
