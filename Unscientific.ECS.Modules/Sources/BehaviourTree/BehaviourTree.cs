using System.Diagnostics.CodeAnalysis;
using Unscientific.BehaviourTree;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTree<TScope>
    {
        private readonly BehaviourTreeNode<Entity<TScope>> _root;
        private readonly BehaviourTreeMetadata<Entity<TScope>> _metadata;
        private readonly BehaviourTreeExecutor<Entity<TScope>> _executor;

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public BehaviourTree(BehaviourTreeNode<Entity<TScope>> root)
        {
            _root = root;
            _metadata = new BehaviourTreeMetadata<Entity<TScope>>(root);
            _executor = new BehaviourTreeExecutor<Entity<TScope>>(root);
        }

        public void Execute(Entity<TScope> entity)
        {
            var data = entity.Get<BehaviourTreeData<TScope>>();
            var executionData = data.ExecutionData;

            if (executionData == null)
            {
                executionData = _metadata.CreateExecutionData();
                entity.Replace(new BehaviourTreeData<TScope>(data.BehaviourTree, executionData));
            }

            if (executionData.Stack.Count == 0)
                _executor.Start(executionData, _root);

            _executor.Tick(executionData, entity);
        }
    }
}
