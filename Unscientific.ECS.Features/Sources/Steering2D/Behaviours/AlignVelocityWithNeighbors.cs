﻿using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public class AlignVelocityWithNeighbors : GroupSteeringBehaviour
    {
        private FixVec2 _averageVelocity;
        private readonly Proximity.Callback _proximityCallback;

        public AlignVelocityWithNeighbors(Proximity proximity) : base(proximity)
        {
            _proximityCallback = ProximityCallback;
        }

        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            _averageVelocity = FixVec2.Zero;

            var neighborCount = Proximity.FindNeighbors(owner, _proximityCallback);

            if (neighborCount > 0)
            {
                var maxVelocity = owner.Get<MaxVelocity>().Value;

                _averageVelocity /= neighborCount;

                if (_averageVelocity.MagnitudeSqr == 0)
                    return SteeringVelocity.Zero;

                _averageVelocity.Normalize();
                _averageVelocity.Scale(ref maxVelocity);
            }

            return new SteeringVelocity(_averageVelocity);
        }

        private bool ProximityCallback(Entity<Game> neighbor, Fix sqrRange)
        {
            _averageVelocity += neighbor.Get<Velocity>().Value;
            return true;
        }
    }
}