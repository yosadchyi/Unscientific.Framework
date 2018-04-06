using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;

namespace Unscientific.ECS.Modules.Steering2D
{
    public abstract class Steering2DModule: IModuleTag
    {
        public class Builder : IModuleBuilder
        {
            private int _updatePeriodInTicks = 1;

            public Builder WithUpdatePeriodInTicks(int updatePeriodInTicks)
            {
                _updatePeriodInTicks = updatePeriodInTicks;
                return this;
            }

            public IModule Build()
            {
                return new Module<Steering2DModule>.Builder()
                        .Usages()
                            .Uses<CoreModule>()
                            .Uses<Physics2DModule>()
                        .End()
                        .Components<Configuration>()
                            .Add<SteeringUpdatePeriod>()
                        .End()
                        .Components<Game>()
                            .Add<Steering>()
                            .Add<FlowField>()
                            .Add<TargetEntity>()
                            .Add<TargetPosition>()
                            .Add<TargetOrientation>()
                            .Add<ArrivalTolerance>()
                            .Add<AlignTolerance>()
                        .End()
                        .Systems()
                            .Add((contexts, messageBus) => new SteeringSetupSystem(contexts, _updatePeriodInTicks))
                            .Add((contexts, messageBus) => new SteeringUpdateSystem(contexts))
                        .End()
                    .Build();
            }
        }
    }
}