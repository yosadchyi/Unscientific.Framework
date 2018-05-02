namespace Unscientific.ECS.Modules.Steering2D
{
    public static class WorldBuilderExtensions
    {
        public static Steering2DModule.Builder UsesSteering2D(this World.Builder self)
        {
            return new Steering2DModule.Builder(self);
        }

        public static World.Builder UsesSteering2DWithDefaults(this World.Builder self)
        {
            return self.UsesSteering2D().End();
        }
    }
}