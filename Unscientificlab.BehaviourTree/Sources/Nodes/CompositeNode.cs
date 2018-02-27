using System.Collections.Generic;

namespace Unscientificlab.BehaviourTree
{
    public abstract class CompositeNode<TBlackboard> : BehaviourTreeNode<TBlackboard>
    {
        private readonly List<BehaviourTreeNode<TBlackboard>> _children = new List<BehaviourTreeNode<TBlackboard>>();

        protected List<BehaviourTreeNode<TBlackboard>> Children
        {
            get { return _children; }
        }

        protected int ChildrenCount
        {
            get { return _children.Count; }
        }

        protected CompositeNode(string name) : base(name)
        {
        }

        public override void DeclareNode(BehaviourTreeMetadata<TBlackboard> metadata)
        {
            base.DeclareNode(metadata);

            foreach (var node in _children)
                node.DeclareNode(metadata);
        }

        public void AddChild(BehaviourTreeNode<TBlackboard> child)
        {
            _children.Add(child);
        }
    }
}