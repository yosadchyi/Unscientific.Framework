﻿using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public class SeparateFromNeighbors : GroupSteeringBehaviour
    {
        private FixVec2 _linear;
        private FixVec2 _position;
        private readonly Proximity.Callback _proximityCallback;

        public SeparateFromNeighbors(Proximity proximity) : base(proximity)
        {
            _proximityCallback = ProximityCallback;
        }

        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            _linear = FixVec2.Zero;
            _position = owner.Get<Position>().Value;

            Proximity.FindNeighbors(owner, _proximityCallback);

            var maxVelocity = owner.Get<MaxVelocity>().Value;

            if (_linear.MagnitudeSqr == 0)
                return SteeringVelocity.Zero;

            _linear.Normalize();
            _linear.Scale(ref maxVelocity);

            return new SteeringVelocity(_linear);
        }

        private bool ProximityCallback(Entity<Game> neighbor, Fix sqrRange)
        {
            var toAgent = _position - neighbor.Get<Position>().Value;

            _linear += toAgent;

            return true;
        }
    }
}