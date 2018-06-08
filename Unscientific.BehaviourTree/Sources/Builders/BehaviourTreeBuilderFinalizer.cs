using System;

namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeBuilderFinalizer<TBlackboard>
    {
        private readonly BehaviourTreeNode<TBlackboard> _node;

        public BehaviourTreeBuilderFinalizer(BehaviourTreeNode<TBlackboard> node)
        {
            _node = node;
        }

        public BehaviourTreeNode<TBlackboard> Build()
        {
            if (_node == null)
                throw new ApplicationException("Can't create tree without node.");
            return _node;
        }
    }
}