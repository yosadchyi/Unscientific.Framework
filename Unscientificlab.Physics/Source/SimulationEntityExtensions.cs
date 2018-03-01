using Unscientificlab.ECS;
using Unscientificlab.ECS.Base;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.Physics
{
    public static class SimulationEntityExtensions
    {
        public static Fix DistanceSqr(this Entity<Simulation> self, Entity<Simulation> other)
        {
            return (other.Get<Position>().Value - self.Get<Position>().Value).MagnitudeSqr;
        }        
    }
}