namespace Unscientific.ECS.Modules.Core
{
    public static class WorldBuilderExtensions {
        public static CoreModule.Builder UsingCoreModule(this World.Builder self)
        {
            return new CoreModule.Builder(self);
        }
        
        public static World.Builder UsingCoreModuleWithDefaults(this World.Builder self)
        {
            return self.UsingCoreModule().End();
        }

    }
}