using Unscientificlab.ECS.Modules.Core;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Physics
{
    public static class SimulationEntityExtensions
    {
        public static Fix DistanceSqr(this Entity<Simulation> self, Entity<Simulation> other)
        {
            return (other.Get<Position>().Value - self.Get<Position>().Value).MagnitudeSqr;
        }        
        
        public static Fix Distance(this Entity<Simulation> self, Entity<Simulation> other)
        {
            return (other.Get<Position>().Value - self.Get<Position>().Value).Magnitude;
        }
    }
}
