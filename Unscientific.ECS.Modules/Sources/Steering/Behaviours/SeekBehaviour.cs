﻿using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering
{
    public class SeekBehaviour : SteeringBehaviour
    {
        private readonly Context<Game> _simulation;

        public SeekBehaviour(Context<Game> simulation)
        {
            _simulation = simulation;
        }

        public override SteeringVelocity DoCalculate(Entity<Game> owner, ref SteeringVelocity accumulatedSteering)
        {
            var target = FixVec2.Zero;

            if (!owner.TryGetTargetPosition(_simulation, ref target))
                return SteeringVelocity.Zero;

            var maxVelocity = owner.Get<MaxVelocity>().Value;
            var position = owner.Get<Position>().Value;

            target.Sub(ref position);

            if (target.MagnitudeSqr == 0)
                return SteeringVelocity.Zero;

            target.Normalize();
            target.Scale(ref maxVelocity);

            return new SteeringVelocity
            {
                Linear = target
            };
        }
    }
}