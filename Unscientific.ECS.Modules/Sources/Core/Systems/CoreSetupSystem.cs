namespace Unscientific.ECS.Modules.Core
{
    public class BaseSetupSystem: ISetupSystem
    {
        private readonly Contexts _contexts;

        public BaseSetupSystem(Contexts contexts)
        {
            _contexts = contexts;
        }

        public void Setup()
        {
            _contexts.Get<Configuration>().CreateEntity().Add(new Singleton());
            var singleton = _contexts.Get<Singletons>().CreateEntity().Add(new Singleton());
            singleton.Add(new Tick(-1));
        }
    }
}
