namespace Unscientific.ECS.Modules.Core
{
    public static class WorldBuilderExtensions {
        public static CoreModule.Builder UsesCoreModule(this World.Builder self)
        {
            return new CoreModule.Builder(self);
        }
        
        public static World.Builder UsesCoreModuleWithDefaults(this World.Builder self)
        {
            return self.UsesCoreModule().End();
        }

    }
}