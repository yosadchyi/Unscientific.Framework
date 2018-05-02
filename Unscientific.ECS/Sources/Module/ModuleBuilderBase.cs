namespace Unscientific.ECS
{
    public abstract class ModuleBuilderBase
    {
        private readonly World.Builder _worldBuilder;

        protected ModuleBuilderBase(World.Builder worldBuilder)
        {
            _worldBuilder = worldBuilder;
        }

        public World.Builder End()
        {
            var module = Build();
            _worldBuilder.Uses(module);
            return _worldBuilder;
        }

        protected abstract IModule Build();
    }
}