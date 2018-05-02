using System;

namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeBuilderNodeFinalizer<TBlackboard, TParent> where TParent: INodeAcceptor<TBlackboard>
    {
        private readonly TParent _parent;
        private readonly BehaviourTreeNode<TBlackboard> _node;

        public BehaviourTreeBuilderNodeFinalizer(TParent parent, BehaviourTreeNode<TBlackboard> node)
        {
            _parent = parent;
            _node = node;
        }

        public TParent End()
        {
            _parent.AcceptNode(_node);
            return _parent;
        }
    }
}