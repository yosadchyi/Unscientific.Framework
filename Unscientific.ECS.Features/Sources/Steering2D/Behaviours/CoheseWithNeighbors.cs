﻿using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public class CoheseWithNeighbors : GroupSteeringBehaviour
    {
        private FixVec2 _centroid;
        private readonly Proximity.Callback _proximityCallback;

        public CoheseWithNeighbors(Proximity proximity) : base(proximity)
        {
            _proximityCallback = ProximityCallback;
        }

        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            _centroid = FixVec2.Zero;

            var neighborCount = Proximity.FindNeighbors(owner, _proximityCallback);

            if (neighborCount > 0)
            {
                _centroid /= neighborCount;
                _centroid -= owner.Get<Position>().Value;

                if (_centroid.MagnitudeSqr == 0)
                    return SteeringVelocity.Zero;

                _centroid.Normalize();

                var maxVelocity = owner.Get<MaxVelocity>().Value;
                _centroid.Scale(ref maxVelocity);
            }

            return new SteeringVelocity(_centroid);
        }

        private bool ProximityCallback(Entity<Game> neighbor, Fix sqrRange)
        {
            _centroid += neighbor.Get<Position>().Value;
            return true;
        }
    }
}