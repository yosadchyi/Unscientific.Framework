using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeModule: IModuleTag
    {
        public class Builder: IModuleBuilder
        {
            public IModule Build()
            {
                return new Module<BehaviourTreeModule>.Builder()
                        .Usages()
                            .Uses<CoreModule>()
                        .End()
                        .Components<Simulation>()
                            .Add<BehaviourTreeData>()
                        .End()
                        .Systems()
                            .Add((contexts, messageBus) => new BehaviourTreeUpdateSystem(contexts))
                            .Add((contexts, messageBus) => new BehaviourTreeCleanupSystem(messageBus))
                        .End()
                    .Build();
            }
        }
    }
}