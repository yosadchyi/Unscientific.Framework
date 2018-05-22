using System;
using Unscientific.ECS.DSL;
using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.ECS.Features.Tick;

namespace Unscientific.ECS.Features.Steering2D
{
    public static class Steering2DFeature
    {
        public const string Name = "Steering2D";

        public class Configurer
        {
            internal int UpdatePeriodInTicks = 1;

            public Configurer SetUpdatePeriodInTicks(int updatePeriodInTicks)
            {
                UpdatePeriodInTicks = updatePeriodInTicks;
                return this;
            }
        }

        public static WorldBuilder AddSteering2DFeature(this WorldBuilder self, Func<Configurer, Configurer> configure)
        {
            var configurer = configure(new Configurer());
            
            // @formatter:off
            return self.AddFeature(Name)
                .DependsOn()
                    .Feature(CoreFeature.Name)
                    .Feature(DestroyFeature.Name)
                    .Feature(TickFeature.Name)
                    .Feature(Physics2DFeature.Name)
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
                    .Setup((contexts, bus) => contexts.Configuration().Add(new SteeringUpdatePeriod(configurer.UpdatePeriodInTicks)))
                    .Update((contexts, bus) => {
                        var updatePeriod = contexts.Configuration().Get<SteeringUpdatePeriod>().PeriodInTicks;
                        var tick = contexts.Singleton().Get<TickCounter>().Value;
                        var context = contexts.Get<Game>();
            
                        if (tick % updatePeriod != 0) return;
            
                        foreach (var entity in context.AllWith<Steering>())
                        {
                            if (entity.Is<Destroyed>()) continue;
            
                            var steeringVelocity = SteeringVelocity.Zero;
                            var steeringBehaviour = entity.Get<Steering>().SteeringBehaviour;
                            var velocity = steeringBehaviour.Calculate(entity, ref steeringVelocity);
            
                            if (entity.Has<Velocity>()) entity.Replace(new Velocity(velocity.Linear));
                            if (entity.Has<AngularVelocity>()) entity.Replace(new AngularVelocity(velocity.Angular));
                        }
                    })
                .End()
            .End();
            // @formatter:on
        }

        public static WorldBuilder AddSteering2DFeature(this WorldBuilder self)
        {
            return self.AddSteering2DFeature(c => c);
        }
    }
}