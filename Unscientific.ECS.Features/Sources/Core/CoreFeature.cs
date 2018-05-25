using System;
using Unscientific.ECS.DSL;
using Unscientific.ECS.Features.Destroy;

namespace Unscientific.ECS.Features.Core
{
    public static class CoreFeature
    {
        public const string Name = "Core";

        public class Configurer
        {
            internal int InitialCapacity = 128;

            public Configurer SetInitialCapacity(int initialCapacity)
            {
                InitialCapacity = initialCapacity;
                return this;
            }
        }

        public static WorldBuilder AddCoreFeature(this WorldBuilder self, Func<Configurer, Configurer> configure)
        {
            var configurer = configure(new Configurer());

            // @formatter:off
            return self.AddFeature(Name)
                    .Contexts()
                        .Add<Game>(c => c.SetInitialCapacity(configurer.InitialCapacity))
                        .Add<Configuration>(c => c.AllowOnlyOneEntity())
                        .Add<Singletons>(c => c.AllowOnlyOneEntity())
                    .End()
                    .Components<Singletons>()
                        .Add<SingletonTag>()
                    .End()
                    .Components<Configuration>()
                        .Add<SingletonTag>()
                    .End()
                    .Systems()
                        .Setup((contexts, bus) => {
                            contexts.Get<Singletons>().CreateEntity().Add(new SingletonTag());
                            contexts.Get<Configuration>().CreateEntity().Add(new SingletonTag());
                        })
                    .End()
                .End()
                .AddDestroyFeature<Game>();
            // @formatter:on
        }

        public static WorldBuilder AddCoreFeature(this WorldBuilder self)
        {
            return self.AddCoreFeature(c => c);
        }
    }
}