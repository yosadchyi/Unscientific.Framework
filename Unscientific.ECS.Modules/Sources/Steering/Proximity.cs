using Unscientific.ECS.Modules.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering
{
    public abstract class Proximity
    {
        public delegate bool Callback (Entity<Game> entity, Fix sqrRange);

        public abstract int FindNeighbors (Entity<Game> entity, Callback callback);
    }
}