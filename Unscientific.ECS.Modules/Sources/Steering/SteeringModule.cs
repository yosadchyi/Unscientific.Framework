using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;

namespace Unscientific.ECS.Modules.Steering
{
    public abstract class SteeringModule: IModuleTag
    {
        public class Builder : IModuleBuilder
        {
            public IModule Build()
            {
                return new Module<SteeringModule>.Builder()
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
                            .Add((contexts, messageBus) => new SteeringSystem(contexts))
                        .End()
                    .Build();
            }
        }
    }
}