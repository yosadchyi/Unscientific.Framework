using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public abstract class BehaviourTreeModule: IModuleTag
    {
        public class Builder: IModuleBuilder
        {
            public IModule Build()
            {
                return new Module<BehaviourTreeModule>.Builder()
                        .Usages()
                            .Uses<CoreModule>()
                        .End()
                        .Components<Game>()
                            .Add<BehaviourTreeData>()
                        .End()
                        .Systems()
                            .Add(contexts => new BehaviourTreeUpdateSystem(contexts))
                            .Add(contexts => new BehaviourTreeCleanupSystem(contexts))
                        .End()
                    .Build();
            }
        }
    }
}