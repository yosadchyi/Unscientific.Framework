﻿using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.ECS.Modules.Physics;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Steering
{
    public class SeparationBehaviour : GroupSteeringBehaviour
    {
        private FixVec2 _linear;
        private readonly Proximity.Callback _proximityCallback;
        private FixVec2 _position;

        public SeparationBehaviour(Proximity proximity) : base(proximity)
        {
            _proximityCallback = ProximityCallback;
        }

        public override SteeringVelocity DoCalculate(Entity<Simulation> owner, ref SteeringVelocity accumulatedSteering)
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

        private bool ProximityCallback(Entity<Simulation> neighbor, Fix sqrRange)
        {
            var toAgent = _position - neighbor.Get<Position>().Value;

            _linear += toAgent;

            return true;
        }
    }
}