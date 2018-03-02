namespace Unscientificlab.ECS.Modules.Base
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
            _contexts.Get<Configuration>().CreateEntity();
            var singleton = _contexts.Get<Singletons>().CreateEntity();
            singleton.Add(new Tick(-1));
        }
    }
}
