using System.Diagnostics.CodeAnalysis;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public abstract class BehaviourTreeModule: IModuleTag
    {
        public class Builder<TScope>: IModuleBuilder where TScope: IScope
        {
            private int _updatePeriodInTicks = 1;

            public Builder<TScope> WithUpdatePeriodInTicks(int updatePeriodInTicks)
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
                            .Add<BehaviourTreeUpdatePeriod<TScope>>()
                        .End()
                        .Components<Game>()
                            .Add<BehaviourTreeData<TScope>>()
                        .End()
                        .Systems()
                            .Add(contexts => new BehaviourTreeSetupSystem<TScope>(contexts, _updatePeriodInTicks))
                            .Add(contexts => new BehaviourTreeUpdateSystem<TScope>(contexts))
                            .Add(contexts => new BehaviourTreeCleanupSystem<TScope>(contexts))
                        .End()
                    .Build();
            }
        }
    }
}