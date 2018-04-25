using System;

namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeBuilderLeaf<TBlackboard> : INodeHandler<TBlackboard>
    {
        private readonly BehaviourTreeNode<TBlackboard> _node;

        public BehaviourTreeBuilderLeaf(BehaviourTreeNode<TBlackboard> node)
        {
            _node = node;
        }

        public BehaviourTreeNode<TBlackboard> Build()
        {
            if (_node == null)
                throw new ApplicationException("Can't create tree without node.");
            DoHandleNode(_node);
            return _node;
        }

        public void DoHandleNode(BehaviourTreeNode<TBlackboard> node)
        {
        }
    }
}