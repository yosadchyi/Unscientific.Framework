using System.Diagnostics.CodeAnalysis;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public abstract class BehaviourTreeModule: IModuleTag
    {
        public class Builder: IModuleBuilder
        {
            private int _updatePeriod = 1;

            public Builder WithUpdatePeriod(int updatePeriod)
            {
                _updatePeriod = updatePeriod;
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
                        .Components<Game>()
                            .Add<BehaviourTreeData>()
                        .End()
                        .Systems()
                            .Add(contexts => new PeriodicUpdateSystem(contexts, new BehaviourTreeUpdateSystem(contexts), _updatePeriod))
                            .Add(contexts => new BehaviourTreeCleanupSystem(contexts))
                        .End()
                    .Build();
            }
        }
    }
}