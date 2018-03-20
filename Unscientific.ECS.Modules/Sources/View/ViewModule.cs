using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.View
{
    public abstract class ViewModule: IModuleTag
    {
        public class Builder: IModuleBuilder
        {
            public IModule Build()
            {
                return new Module<ViewModule>.Builder()
                    .Components<Game>()
                        .Add<View>()
                    .End()
                    .Notifications<Game>()
                        .AddComponentNotifications<View>()
                    .End()
                .Build();
            }
        }
    }
}