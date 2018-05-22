using System;
using Unscientific.ECS.DSL;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Destroy;
using Unscientific.ECS.Modules.Tick;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public static class Physics2DFeature
    {
        public const string Name = "Physics2D";

        public interface ISetTimeStep
        {
            IAddSpatialDatabase SetTimeStep(Fix timeStep);
        }
        
        public interface IAddSpatialDatabase
        {
            IConfigurationEnd AddSpatialDatabase(ISpatialDatabase spatialDatabase);
        }

        public interface IConfigurationEnd
        {
        }

        public class Configurer : ISetTimeStep, IAddSpatialDatabase, IConfigurationEnd
        {
            internal Fix TimeStep;
            internal ISpatialDatabase SpatialDatabase;

            public IAddSpatialDatabase SetTimeStep(Fix timeStep)
            {
                TimeStep = timeStep;
                return this;
            }

            public IConfigurationEnd AddSpatialDatabase(ISpatialDatabase spatialDatabase)
            {
                SpatialDatabase = spatialDatabase;
                return this;
            }
        }

        public static WorldBuilder AddPhysics2DFeature(this WorldBuilder self, Func<ISetTimeStep, IConfigurationEnd> configure)
        {
            var configurer = new Configurer();

            configure(configurer);

            // @formatter:off
            return self.AddFeature(Name)
                .DependsOn()
                    .Feature(CoreFeature.Name)
                    .Feature(DestroyFeature.Name)
                    .Feature(TickFeature.Name)
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
                    .Setup((contexts, bus) => SetupSystem.Setup(contexts, configurer.TimeStep, configurer.SpatialDatabase))
                    .Update((contexts, bus) => AccelerateSystem.Update(contexts))
                    .Update((contexts, bus) => AngularAccelerateSystem.Update(contexts))
                    .Update((contexts, bus) => MoveSystem.Update(contexts))
                    .Update((contexts, bus) => RotateSystem.Update(contexts))
                    .Update((contexts, bus) => ProcessCollisionsSystem.Update(contexts))
                    .Cleanup((contexts, bus) => CollisionsCleanupSystem.Cleanup(contexts))
                    .Cleanup((contexts, bus) => SpatialDatanaseCleanupSystem.Cleanup(contexts))
                    .Cleanup((contexts, bus) => ReturnCollisionsListOnDestroySystem.Cleanup(contexts))
                .End()
            .End();
            // @formatter:on
        }
    }
}