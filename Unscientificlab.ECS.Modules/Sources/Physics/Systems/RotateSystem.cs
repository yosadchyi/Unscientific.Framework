﻿using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics.Systems
{
    public class RotateSystem: IUpdateSystem
    {
        private readonly Entity<Configuration> _configuration;
        private readonly Context<Simulation> _simulation;

        public RotateSystem(Contexts contexts)
        {
            _configuration = contexts.Get<Configuration>().First();
            _simulation = contexts.Get<Simulation>();
        }

        public void Update()
        {
            var dt = _configuration.Get<TimeStep>().Value;

            foreach (var entity in _simulation.AllWith<Orientation, AngularVelocity>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                var orientation = entity.Get<Orientation>().Value;
                var angularVelocity = entity.Get<AngularVelocity>().Value;

                entity.Replace(new Orientation(orientation + angularVelocity * dt));
            }
        }
    }
}