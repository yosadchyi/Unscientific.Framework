using Unscientific.ECS.Modules.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public static class SimulationEntityExtensions
    {
        public static Fix DistanceSqr(this Entity<Game> self, Entity<Game> other)
        {
            return (other.Get<Position>().Value - self.Get<Position>().Value).MagnitudeSqr;
        }        
        
        public static Fix Distance(this Entity<Game> self, Entity<Game> other)
        {
            return (other.Get<Position>().Value - self.Get<Position>().Value).Magnitude;
        }
    }
}
