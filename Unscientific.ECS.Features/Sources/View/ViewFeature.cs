﻿using Unscientific.ECS.DSL;
using Unscientific.ECS.Features.Core;

namespace Unscientific.ECS.Features.View
{
    public static class ViewFeature
    {
        public const string Name = "View";

        public static WorldBuilder AddViewFeature(this WorldBuilder self)
        {
            // @formatter:off
            return self.AddFeature(Name)
                .DependsOn()
                    .Feature(CoreFeature.Name)
                .End()
                .Components<Game>()
                    .Add<View>()
                .End()
                .GlobalComponentNotifications<Game>()
                    .AddAllNotifications<View>()
                .End()
            .End();
            // @formatter:on
        } 
    }
}