using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Steering
{
    public abstract class Proximity
    {
        public delegate bool Callback (Entity<Simulation> entity, Fix sqrRange);

        public abstract int FindNeighbors (Entity<Simulation> entity, Callback callback);
    }
}