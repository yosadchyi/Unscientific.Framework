using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;

namespace Unscientific.ECS.Modules.Steering2D
{
    public abstract class Steering2DModule: IModuleTag
    {
        public class Builder : IModuleBuilder
        {
            private int _updatePeriod = 1;

            public Builder WithUpdatePeriod(int updatePeriod)
            {
                _updatePeriod = updatePeriod;
                return this;
            }

            public IModule Build()
            {
                return new Module<Steering2DModule>.Builder()
                        .Usages()
                            .Uses<CoreModule>()
                            .Uses<Physics2DModule>()
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
                            .Add((contexts, messageBus) => new PeriodicUpdateSystem(contexts, new SteeringSystem(contexts), _updatePeriod))
                        .End()
                    .Build();
            }
        }
    }
}