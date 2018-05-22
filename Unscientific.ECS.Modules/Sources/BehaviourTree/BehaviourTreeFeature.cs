using System;
using Unscientific.ECS.DSL;
using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.ECS.Features.Tick;

namespace Unscientific.ECS.Features.BehaviourTree
{
    public static class BehaviourTreeFeature
    {
        public const string Name = "BehaviourTree";

        public class Configurer
        {
            internal int UpdatePeriodInTicks = 1;

            public Configurer SetUpdatePeriodInTicks(int updatePeriodInTicks)
            {
                UpdatePeriodInTicks = updatePeriodInTicks;
                return this;
            }
        }

        public static WorldBuilder AddBehaviourTreeFeature<TScope>(this WorldBuilder self, Func<Configurer, Configurer> configure)
        {
            var configurer = configure(new Configurer());

            // @formatter:off
            return self.AddFeature(Name)
                .Components<Configuration>()
                    .Add<BehaviourTreeUpdatePeriod<TScope>>()
                .End()
                .Components<Game>()
                    .Add<BehaviourTreeData<TScope>>()
                .End()
                .Systems()
                    .Setup((contexts, bus) => contexts.Configuration().Add(new BehaviourTreeUpdatePeriod<TScope>(configurer.UpdatePeriodInTicks)))
                    .Update((contexts, bus) => {
                        var updatePeriod = contexts.Configuration().Get<BehaviourTreeUpdatePeriod<TScope>>().PeriodInTicks;
                        var tick = contexts.Singleton().Get<TickCounter>().Value;
                        var context = contexts.Get<TScope>();

                        if (tick % updatePeriod != 0) return;
                
                        foreach (var entity in context.AllWith<BehaviourTreeData<TScope>>())
                        {
                            if (!entity.Is<Destroyed>()) entity.Get<BehaviourTreeData<TScope>>().BehaviourTree.Execute(entity);
                        }
                    })
                    .Cleanup((contexts, bus) => {
                        var context = contexts.Get<TScope>();

                        foreach (var entity in context.AllWith<Destroyed, BehaviourTreeData<TScope>>())
                        {
                            var data = entity.Get<BehaviourTreeData<TScope>>().ExecutionData;

                            data.Return();
                            entity.Remove<BehaviourTreeData<TScope>>();
                        }
                    })
                .End()
            .End();
            // @formatter:on
        }

        public static WorldBuilder AddBehaviourTreeFeature<TScope>(this WorldBuilder self)
        {
            return self.AddBehaviourTreeFeature<TScope>(c => c);
        }
    }
}