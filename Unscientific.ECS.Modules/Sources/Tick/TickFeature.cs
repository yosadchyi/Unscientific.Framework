using Unscientific.ECS.DSL;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Tick
{
    public static class TickFeature
    {
        public const string Name = "Tick";

        public static WorldBuilder AddTickFeature(this WorldBuilder self)
        {
            // @formatter:off
            return self.AddFeature(Name)
                .DependsOn()
                    .Feature(CoreFeature.Name)
                .End()
                .Components<Singletons>()
                    .Add<TickCounter>()
                .End()
                .Systems()
                    .Setup((contexts, bus) => contexts.Singleton().Add(new TickCounter(-1)))
                    .Update((contexts, bus) => {
                        var singleton = contexts.Singleton();
                        var value = singleton.Get<TickCounter>().Value;
        
                        singleton.Replace(new TickCounter(value + 1));
                    })
                .End()
            .End();
            // @formatter:on
        }
    }
}