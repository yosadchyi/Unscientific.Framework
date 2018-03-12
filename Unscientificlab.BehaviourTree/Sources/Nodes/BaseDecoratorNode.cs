namespace Unscientific.BehaviourTree
{
    public abstract class BaseDecoratorNode<TBlackboard> : BehaviourTreeNode<TBlackboard>
    {
        public BehaviourTreeNode<TBlackboard> Node { get; set; }

        protected BaseDecoratorNode(string name) : base(name)
        {
        }

        public override void DeclareNode(BehaviourTreeMetadata<TBlackboard> metadata)
        {
            base.DeclareNode(metadata);
            Node.DeclareNode(metadata);
        }
    }
}