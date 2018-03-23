using System;
using Unscientific.Util.Collections;
using Unscientific.Util.Pool;

namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeExecutionData<TBlackboard>
    {
        public class Pool : ObjectPool<BehaviourTreeExecutionData<TBlackboard>>
        {
            private readonly BehaviourTreeMetadata<TBlackboard> _metadata;

            public Pool(BehaviourTreeMetadata<TBlackboard> metadata)
            {
                _metadata = metadata;
            }

            protected override void Deactivate(BehaviourTreeExecutionData<TBlackboard> instance)
            {
                instance.Deactivate();
            }

            protected override BehaviourTreeExecutionData<TBlackboard> CreateInstance()
            {
                return new BehaviourTreeExecutionData<TBlackboard>(_metadata, this);
            }
        }

        private readonly Pool _pool;
        public readonly int[] Variables;
        public readonly BehaviourTreeStatus[] Statuses;
        public readonly Deque<BehaviourTreeNode<TBlackboard>> Stack;

        public BehaviourTreeStatus GetStatus(BehaviourTreeNode<TBlackboard> node)
        {
            return Statuses[node.Id];
        }

        public void SetStatus(BehaviourTreeNode<TBlackboard> node, BehaviourTreeStatus status)
        {
            Statuses[node.Id] = status;
        }

        public BehaviourTreeExecutionData(BehaviourTreeMetadata<TBlackboard> metadata, Pool pool)
        {
            _pool = pool;
            Stack = new Deque<BehaviourTreeNode<TBlackboard>>(metadata.NodesCount);
            Variables = new int[metadata.VariablesCount];
            Statuses = new BehaviourTreeStatus[metadata.NodesCount];
        }

        private void Deactivate()
        {
            Array.Clear(Statuses, 0, Statuses.Length);
            Stack.Clear();
        }

        public void Return()
        {
            _pool.Return(this);
        }
    }
}