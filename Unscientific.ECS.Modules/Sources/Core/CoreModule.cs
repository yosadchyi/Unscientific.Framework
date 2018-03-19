namespace Unscientific.ECS.Modules.Core
{
    public abstract class CoreModule: IModuleTag
    {
        public class Builder : IModuleBuilder
        {
            private int _initialCapacity = 128;
            
            public Builder WithInitialSimulationCapacity(int initialSimulationCapacity)
            {
                _initialCapacity = initialSimulationCapacity;
                return this;
            }

            public IModule Build()
            {
                return new Module<CoreModule>.Builder()
                        .Contexts()
                            .Add<Simulation>(_initialCapacity)
                            .Add<Configuration>(1, 1)
                            .Add<Singletons>(1, 1)
                        .End()
                        .Components<Simulation>()
                            .Add<Destroyed>()
                        .End()
                        .Components<Singletons>()
                            .Add<Tick>()
                        .End()
                        .Messages()
                            .Add<EntityDestroyed<Simulation>>(_initialCapacity)
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