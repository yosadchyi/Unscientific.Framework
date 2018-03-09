using Unscientificlab.ECS.Modules.Core;

namespace Unscientificlab.ECS.Modules.Physics
{
    public class PhysicsModule: IModule
    {
        private readonly ISpatialDatabase _spatialDatabase;

        public PhysicsModule(ISpatialDatabase spatialDatabase)
        {
            _spatialDatabase = spatialDatabase;
        }

        public ModuleImports Imports()
        {
            return new ModuleImports()
                .Import<CoreModule>();
        }

        public ContextRegistrations Contexts()
        {
            return new ContextRegistrations()
                .Add<Simulation>()
                .Add<Singletons>()
                .Add<Configuration>();
        }

        public MessageRegistrations Messages()
        {
            return new MessageRegistrations()
                .Add<EntitiesCollided>();
        }

        public ComponentRegistrations Components()
        {
            return new ComponentRegistrations()
                .For<Simulation>()
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
                .For<Configuration>()
                    .Add<TimeStep>()
                .End()
                .For<Singletons>()
                    .Add<Space>()
                .End();
        }

        public Systems Systems(Contexts contexts, MessageBus bus)
        {
            return new Systems.Builder()
                .Add(new SpaceSetupSystem(contexts, _spatialDatabase))
                .Add(new AccelerateSystem(contexts))
                .Add(new AngularAccelerateSystem(contexts))
                .Add(new MoveSystem(contexts))
                .Add(new RotateSystem(contexts))
                .Add(new ProcessCollisionSystem(contexts, bus))
                .Add(new CollisionsCleanupSystem(contexts))
                .Add(new ShapesCleanupSystem(bus))
                .Build();
        }
    }
}