using System.Diagnostics.CodeAnalysis;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public abstract class BehaviourTreeModule: IModuleTag
    {
        public class Builder: IModuleBuilder
        {
            private int _updatePeriodInTicks = 1;

            public Builder WithUpdatePeriodInTicks(int updatePeriodInTicks)
            {
                _updatePeriodInTicks = updatePeriodInTicks;
                return this;
            }

            [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
            [SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
            public IModule Build()
            {
                return new Module<BehaviourTreeModule>.Builder()
                        .Usages()
                            .Uses<CoreModule>()
                        .End()
                        .Components<Configuration>()
                            .Add<BehaviourTreeUpdatePeriod>()
                        .End()
                        .Components<Game>()
                            .Add<BehaviourTreeData>()
                        .End()
                        .Systems()
                            .Add(contexts => new BehaviourTreeSetupSystem(contexts, _updatePeriodInTicks))
                            .Add(contexts => new BehaviourTreeUpdateSystem(contexts))
                            .Add(contexts => new BehaviourTreeCleanupSystem(contexts))
                        .End()
                    .Build();
            }
        }
    }
}