using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.View
{
    public abstract class ViewModule: IModuleTag
    {
        public class Builder: ModuleBuilderBase
        {
            public Builder(World.Builder worldBuilder) : base(worldBuilder)
            {
            }

            protected override IModule Build()
            {
                return new Module<ViewModule>.Builder()
                    .Components<Game>()
                        .Add<View>()
                    .End()
                    .ComponentNotifications<Game>()
                        .AddAllNotifications<View>()
                    .End()
                .Build();
            }
        }
    }
}