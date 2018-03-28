using System;
using Unscientific.ECS.Modules.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public abstract class PhysicsModule: IModuleTag
    {
        public class Builder : IModuleBuilder
        {
            private ISpatialDatabase _spatialDatabase;
            private Fix _timeStep = Fix.Ratio(1, 60);
    
            public Builder WithSpatialDatabase(ISpatialDatabase spatialDatabase)
            {
                _spatialDatabase = spatialDatabase;
                return this;
            }

            public Builder WithTimeStep(Fix timeStep)
            {
                _timeStep = timeStep;
                return this;
            }
            
            public IModule Build()
            {
                if (_spatialDatabase == null)
                    throw new ArgumentException("Spatial Database not specified!");
    
                return new Module<PhysicsModule>.Builder()
                    .Usages()
                        .Uses<CoreModule>()
                    .End()
                    .Messages()
                        .Add<EntitiesCollided>()
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
                        .Add<BoundingShape>()
                        .Add<Collisions>()
                    .End()
                    .Components<Configuration>()
                        .Add<TimeStep>()
                    .End()
                    .Components<Singletons>()
                        .Add<Space>()
                    .End()
                    .ComponentNotifications<Game>()
                        .AddAllNotifications<Position>()
                        .AddAllNotifications<Orientation>()
                    .End()
                    .Systems()
                        .Add((contexts, messageBus) => new SetupSystem(contexts, _spatialDatabase, _timeStep))
                        .Add((contexts, messageBus) => new AccelerateSystem(contexts))
                        .Add((contexts, messageBus) => new AngularAccelerateSystem(contexts))
                        .Add((contexts, messageBus) => new MoveSystem(contexts))
                        .Add((contexts, messageBus) => new RotateSystem(contexts))
                        .Add((contexts, messageBus) => new ProcessCollisionSystem(contexts, messageBus))
                        .Add((contexts, messageBus) => new CollisionsCleanupSystem(contexts))
                        .Add((contexts, messageBus) => new ShapesCleanupSystem(contexts))
                    .End()
                .Build();
            }
        }

    }
}