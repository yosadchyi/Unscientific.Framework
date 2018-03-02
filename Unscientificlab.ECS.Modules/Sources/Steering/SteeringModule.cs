using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.ECS.Modules.Physics;

namespace Unscientificlab.ECS.Modules.Steering
{
    public abstract class SteeringModule: AbstractModule
    {
        public override ModuleImports Imports()
        {
            return base.Imports()
                .Import<BaseModule>()
                .Import<PhysicsModule>();
        }

        public override ComponentRegistrations Components()
        {
            return base.Components()
                .For<Simulation>()
                    .Add<Steering>()
                    .Add<FlowField>()
                    .Add<TargetEntity>()
                    .Add<TargetPosition>()
                    .Add<TargetOrientation>()
                    .Add<ArrivalTolerance>()
                    .Add<AlignTolerance>()
                .End();

        }

        public override Systems Systems(Contexts contexts, MessageBus bus)
        {
            return new Systems.Builder()
                .Add(new SteeringSystem(contexts))
                .Build();
        }
    }
}