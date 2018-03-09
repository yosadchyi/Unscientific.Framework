using Unscientificlab.BehaviourTree;
using Unscientificlab.ECS.Modules.Core;

namespace Unscientificlab.ECS.Modules.BehaviourTree
{
    public class BehaviourTree
    {
        public readonly BehaviourTreeNode<Entity<Simulation>> Root;
        public readonly BehaviourTreeMetadata<Entity<Simulation>> Metadata;
        public readonly BehaviourTreeExecutor<Entity<Simulation>> Executor;

        public BehaviourTree(BehaviourTreeNode<Entity<Simulation>> root)
        {
            Root = root;
            Metadata = new BehaviourTreeMetadata<Entity<Simulation>>(root);
            Executor = new BehaviourTreeExecutor<Entity<Simulation>>(root);
        }

        public void Execute(Entity<Simulation> entity)
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
