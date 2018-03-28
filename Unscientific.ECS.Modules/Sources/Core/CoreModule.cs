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
                            .Add<Game>(_initialCapacity)
                            .Add<Configuration>(1, 1)
                            .Add<Singletons>(1, 1)
                        .End()
                        .Components<Game>()
                            .Add<Destroyed>()
                        .End()
                        .Components<Singletons>()
                            .Add<Singleton>()
                            .Add<Tick>()
                        .End()
                        .Components<Configuration>()
                            .Add<Singleton>()
                        .End()
                        .ComponentNotifications<Game>()
                            .AddAddedNotifications<Destroyed>()
                        .End()
                        .Systems()
                            .Add((contexts, messageBus) => new BaseSetupSystem(contexts))
                            .Add((contexts, messageBus) => new IncrementTickSystem(contexts))
                            .Add((contexts, messageBus) => new DestroySystem<Game>(contexts, messageBus))
                        .End()
                    .Build();
            }
        }
    }

}