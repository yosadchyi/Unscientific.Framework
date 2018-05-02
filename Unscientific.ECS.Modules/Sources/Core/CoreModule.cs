namespace Unscientific.ECS.Modules.Core
{
    public abstract class CoreModule: IModuleTag
    {
        public class Builder : ModuleBuilderBase
        {
            private int _initialCapacity = 128;

            public Builder(World.Builder worldBuilder) : base(worldBuilder)
            {
            }

            public Builder WithInitialSimulationCapacity(int initialSimulationCapacity)
            {
                _initialCapacity = initialSimulationCapacity;
                return this;
            }

            protected override IModule Build()
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
                            .Add<SingletonTag>()
                            .Add<Tick>()
                        .End()
                        .Components<Configuration>()
                            .Add<SingletonTag>()
                        .End()
                        .ComponentNotifications<Game>()
                            .AddAddedNotifications<Destroyed>()
                        .End()
                        .Systems()
                            .Add(contexts => new BaseSetupSystem(contexts))
                            .Add(contexts => new IncrementTickSystem(contexts))
                            .Add((contexts, messageBus) => new DestroySystem<Game>(contexts, messageBus))
                        .End()
                    .Build();
            }
        }
    }
}