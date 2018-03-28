using System.Diagnostics.CodeAnalysis;
using Unscientific.BehaviourTree;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTree
    {
        public readonly BehaviourTreeNode<Entity<Game>> Root;
        public readonly BehaviourTreeMetadata<Entity<Game>> Metadata;
        public readonly BehaviourTreeExecutor<Entity<Game>> Executor;

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public BehaviourTree(BehaviourTreeNode<Entity<Game>> root)
        {
            Root = root;
            Metadata = new BehaviourTreeMetadata<Entity<Game>>(root);
            Executor = new BehaviourTreeExecutor<Entity<Game>>(root);
        }

        public void Execute(Entity<Game> entity)
        {
            if (entity.Is<Destroyed>())
                return;

            var data = entity.Get<BehaviourTreeData>();
            var executionData = data.ExecutionData;

            if (executionData == null)
            {
                executionData = Metadata.CreateExecutionData();
                entity.Replace(new BehaviourTreeData(data.BehaviourTree, executionData));
            }

            if (executionData.Stack.Count == 0)
                Executor.Start(executionData, Root);

            Executor.Tick(executionData, entity);
        }
    }
}
