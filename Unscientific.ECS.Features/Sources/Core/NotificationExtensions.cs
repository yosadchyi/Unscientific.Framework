using Unscientific.ECS.DSL;

namespace Unscientific.ECS.Features.Core
{
    public static class NotificationExtensions
    {
        public static ComponentNotificationsBuilder<TScope> ComponentNotifications<TScope>(this FeatureBuilder self)
        {
            return new ComponentNotificationsBuilder<TScope>(self);
        }

        public static GlobalComponentNotificationsBuilder<TScope> GlobalComponentNotifications<TScope>(this FeatureBuilder self)
        {
            return new GlobalComponentNotificationsBuilder<TScope>(self);
        }
        
        public static MessageNotificationsBuilder MessageNotifications(this FeatureBuilder self)
        {
            return new MessageNotificationsBuilder(self);
        }
    }
}