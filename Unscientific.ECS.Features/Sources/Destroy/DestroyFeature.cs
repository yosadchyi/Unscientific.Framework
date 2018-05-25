using Unscientific.ECS.DSL;
using Unscientific.ECS.Features.Core;

namespace Unscientific.ECS.Features.Destroy
{
    public static class DestroyFeature
    {
        public const string Name = "Destroy";

        public static WorldBuilder AddDestroyFeature<TScope>(this WorldBuilder self)
        {
            // @formatter:off
            return self.AddFeature(Name)
                .DependsOn()
                    .Feature(CoreFeature.Name)
                .End()
                .Components<Game>()
                    .Add<Destroyed>()
                .End()
                .GlobalComponentNotifications<TScope>()
                    .AddAddedNotifications<Destroyed>()
                .End()
                .Systems()
                    .Cleanup((contexts, bus) => {
                        var context = contexts.Get<TScope>();

                        foreach (var message in bus.All<ComponentAdded<TScope, Destroyed>>())
                        {
                            context.DestroyEntity(message.Entity);
                        }
                    })
                .End()
            .End();
            // @formatter:on
        }
    }
}