namespace Unscientificlab.ECS.Base
{
    public class SetupSystem: ISetupSystem
    {
        private readonly Contexts _contexts;

        public SetupSystem(Contexts contexts)
        {
            _contexts = contexts;
        }

        public void Setup()
        {
            _contexts.Get<Configuration>().CreateEntity();
            var singleton = _contexts.Get<Singletons>().CreateEntity();
            singleton.Add(new Tick(0));
        }
    }
}
