using Unscientific.ECS.Features.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public abstract class Proximity
    {
        public delegate bool Callback (Entity<Game> entity, Fix sqrRange);

        public abstract int FindNeighbors (Entity<Game> entity, Callback callback);
    }
}