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
            _contexts.Get<Configuration>().CreateEntity().Add(new SingletonTag());
            var singleton = _contexts.Get<Singletons>().CreateEntity().Add(new SingletonTag());
            singleton.Add(new Tick(-1));
        }
    }
}
