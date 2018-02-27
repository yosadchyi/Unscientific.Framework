using System.Collections.Generic;

namespace Unscientificlab.BehaviourTree
{
    public class BehaviourTreeMetadata<TBlackboard>
    {
        public int NodesCount { get { return _nodes.Count; } }
        
        public List<BehaviourTreeNode<TBlackboard>> Nodes { get { return _nodes; } }

        private readonly List<BehaviourTreeNode<TBlackboard>> _nodes = new List<BehaviourTreeNode<TBlackboard>>();

        public class VariableDefinition
        {
            public readonly BehaviourTreeNode<TBlackboard> Owner;
            public readonly string Name;

            public VariableDefinition(BehaviourTreeNode<TBlackboard> owner, string name)
            {
                Owner = owner;
                Name = name;
            }
        }

        public int VariablesCount { get { return _variableDefinitions.Count; } }

        private readonly List<VariableDefinition> _variableDefinitions = new List<VariableDefinition>();

        private readonly BehaviourTreeExecutionData<TBlackboard>.Pool _pool;

        public BehaviourTreeMetadata(BehaviourTreeNode<TBlackboard> root)
        {
            root.DeclareNode(this);
            _pool = new BehaviourTreeExecutionData<TBlackboard>.Pool(this);
        }

        public BehaviourTreeExecutionData<TBlackboard> CreateExecutionData()
        {
            return _pool.Get();
        }
        
        public int DeclareVariable(BehaviourTreeNode<TBlackboard> node, string name)
        {
            var definition = new VariableDefinition(node, name);
            var index = VariablesCount;

            _variableDefinitions.Add(definition);
            return index;
        }

        public int DeclareNode(BehaviourTreeNode<TBlackboard> node)
        {
            var index = NodesCount;
            _nodes.Add(node);
            return index;
        }
    }
}
