﻿namespace Unscientific.ECS.Modules.View
{
    public static class WorldBuilderExtensions
    {
        public static World.Builder UsingViewModuleWithDefaults(this World.Builder self)
        {
            return new ViewModule.Builder(self).End();
        }
    }
}