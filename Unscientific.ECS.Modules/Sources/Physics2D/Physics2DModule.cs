﻿using System;
using Unscientific.ECS.Modules.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public abstract class Physics2DModule: IModuleTag
    {
        public interface IWithTimeStep
        {
            IWithSpatialDatabase WithTimeStep(Fix timeStep);
        }
        
        public interface IWithSpatialDatabase
        {
            Builder WithSpatialDatabase(ISpatialDatabase spatialDatabase);
        }

        public class Builder : ModuleBuilderBase, IWithTimeStep, IWithSpatialDatabase
        {
            private ISpatialDatabase _spatialDatabase;
            private Fix _timeStep = Fix.Ratio(1, 50);

            public Builder(World.Builder worldBuilder) : base(worldBuilder)
            {
            }

            public IWithSpatialDatabase WithTimeStep(Fix timeStep)
            {
                _timeStep = timeStep;
                return this;
            }

            public Builder WithSpatialDatabase(ISpatialDatabase spatialDatabase)
            {
                _spatialDatabase = spatialDatabase;
                return this;
            }

            protected override IModule Build()
            {
                if (_spatialDatabase == null)
                    throw new ArgumentException("Spatial Database not specified!");
    
                return new Module<Physics2DModule>.Builder()
                    .Usages()
                        .Uses<CoreModule>()
                    .End()
                    .Components<Game>()
                        .Add<Position>()
                        .Add<Velocity>()
                        .Add<Damping>()
                        .Add<Force>()
                        .Add<MaxVelocity>()
                        .Add<Mass>()
                        .Add<Orientation>()
                        .Add<AngularVelocity>()
                        .Add<MaxAngularVelocity>()
                        .Add<AngularDamping>()
                        .Add<Torque>()
                        .Add<Inertia>()
                        .Add<BoundingShapes>()
                        .Add<Collisions>()
                    .End()
                    .Components<Configuration>()
                        .Add<TimeStep>()
                        .Add<GlobalForce>()
                    .End()
                    .Components<Singletons>()
                        .Add<Space>()
                    .End()
                    .ComponentNotifications<Game>()
                        .AddAllNotifications<Position>()
                        .AddAllNotifications<Orientation>()
                    .End()
                    .Systems()
                        .Add(contexts => new SetupSystem(contexts, _spatialDatabase, _timeStep))
                        .Add(contexts => new AccelerateSystem(contexts))
                        .Add(contexts => new AngularAccelerateSystem(contexts))
                        .Add(contexts => new MoveSystem(contexts))
                        .Add(contexts => new RotateSystem(contexts))
                        .Add(contexts => new ProcessCollisionsSystem(contexts))
                        .Add(contexts => new CollisionsCleanupSystem(contexts))
                        .Add(contexts => new SpatialDatanaseCleanupSystem(contexts))
                    .End()
                .Build();
            }
        }
    }
}