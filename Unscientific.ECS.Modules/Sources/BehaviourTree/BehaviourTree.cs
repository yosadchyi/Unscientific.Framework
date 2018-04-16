using System.Diagnostics.CodeAnalysis;
using Unscientific.BehaviourTree;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTree<TScope> where TScope : IScope
    {
        public readonly BehaviourTreeNode<Entity<TScope>> Root;
        public readonly BehaviourTreeMetadata<Entity<TScope>> Metadata;
        public readonly BehaviourTreeExecutor<Entity<TScope>> Executor;

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public BehaviourTree(BehaviourTreeNode<Entity<TScope>> root)
        {
            Root = root;
            Metadata = new BehaviourTreeMetadata<Entity<TScope>>(root);
            Executor = new BehaviourTreeExecutor<Entity<TScope>>(root);
        }

        public void Execute(Entity<TScope> entity)
        {
            var data = entity.Get<BehaviourTreeData<TScope>>();
            var executionData = data.ExecutionData;

            if (executionData == null)
            {
                executionData = Metadata.CreateExecutionData();
                entity.Replace(new BehaviourTreeData<TScope>(data.BehaviourTree, executionData));
            }

            if (executionData.Stack.Count == 0)
                Executor.Start(executionData, Root);

            Executor.Tick(executionData, entity);
        }
    }
}
