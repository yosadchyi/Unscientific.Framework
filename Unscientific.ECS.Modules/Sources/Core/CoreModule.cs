namespace Unscientific.ECS.Modules.Core
{
    public class CoreModule: IModuleTag
    {
        public class Builder : IModuleBuilder
        {
            public IModule Build()
            {
                return new Module<CoreModule>.Builder()
                        .Contexts()
                            .Add<Simulation>()
                            .Add<Configuration>()
                            .Add<Singletons>()
                        .End()
                        .Components<Simulation>()
                            .Add<Destroyed>()
                        .End()
                        .Components<Singletons>()
                            .Add<Tick>()
                        .End()
                        .Messages()
                            .Add<EntityDestroyed<Simulation>>()
                        .End()
                        .Systems()
                            .Add((contexts, messageBus) => new BaseSetupSystem(contexts))
                            .Add((contexts, messageBus) => new IncrementTickSystem(contexts))
                            .Add((contexts, messageBus) => new DestroySystem<Simulation>(contexts, messageBus))
                        .End()
                    .Build();
            }
        }
    }

}