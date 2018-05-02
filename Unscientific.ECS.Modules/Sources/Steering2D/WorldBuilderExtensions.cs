namespace Unscientific.ECS.Modules.Steering2D
{
    public static class WorldBuilderExtensions
    {
        public static Steering2DModule.Builder UsingSteering2D(this World.Builder self)
        {
            return new Steering2DModule.Builder(self);
        }

        public static World.Builder UsingSteering2DWithDefaults(this World.Builder self)
        {
            return self.UsingSteering2D().End();
        }
    }
}