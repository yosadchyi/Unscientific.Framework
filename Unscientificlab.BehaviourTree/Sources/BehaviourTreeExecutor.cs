namespace Unscientificlab.BehaviourTree
{
    public class BehaviourTreeExecutor<TBlackboard>
    {
        private readonly BehaviourTreeNode<TBlackboard> _root;

        public BehaviourTreeExecutor(BehaviourTreeNode<TBlackboard> root)
        {
            _root = root;
        }

        public void Start(BehaviourTreeExecutionData<TBlackboard> data)
        {
            Start(data, _root);
        }

        public void Start(BehaviourTreeExecutionData<TBlackboard> data,
            BehaviourTreeNode<TBlackboard> node,
            ICompletionObserver<TBlackboard> observer = null)
        {
            if (observer != null)
                node.Observer = observer;
            data.Stack.AddFront(node);
        }

        public void Schedule(BehaviourTreeExecutionData<TBlackboard> data,
            BehaviourTreeNode<TBlackboard> node,
            ICompletionObserver<TBlackboard> observer = null)
        {
            if (observer != null)
                node.Observer = observer;
            data.Stack.AddBack(node);
        }

        public void Abort(BehaviourTreeExecutionData<TBlackboard> data, BehaviourTreeNode<TBlackboard> node)
        {
            RemoveNode(data, node);
            node.Abort(data);
        }

        private static void RemoveNode(BehaviourTreeExecutionData<TBlackboard> data,
            BehaviourTreeNode<TBlackboard> node)
        {
            var index = data.Stack.IndexOf(node);

            if (index >= 0)
                data.Stack.RemoveAt(index);
        }

        public void Stop(BehaviourTreeExecutionData<TBlackboard> data,
            BehaviourTreeNode<TBlackboard> node,
            BehaviourTreeStatus status)
        {
            RemoveNode(data, node);
            data.Statuses[node.Id] = status;

            if (node.Observer != null)
                node.Observer.OnComplete(this, data, status);
        }

        public void Tick(BehaviourTreeExecutionData<TBlackboard> executionData, TBlackboard blackboard)
        {
            executionData.Stack.AddBack(null);

            while (Step(executionData, blackboard))
            {
            }
        }

        private bool Step(BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            var current = data.Stack.RemoveFront();

            if (current == null)
                return false;

            var status = current.Tick(this, data, blackboard);


            if (status != BehaviourTreeStatus.Running)
            {
                if (current.Observer != null)
                    current.Observer.OnComplete(this, data, status);
            }
            else
            {
                data.Stack.AddBack(current);
            }

            return true;
        }
    }
}