namespace Unscientific.ECS.Modules.Physics2D
{
    public static class WorldBuilderExtensions
    {
        public static Physics2DModule.IWithTimeStep UsesPhysics2D(this World.Builder self)
        {
            return new Physics2DModule.Builder(self);
        }
    }
}