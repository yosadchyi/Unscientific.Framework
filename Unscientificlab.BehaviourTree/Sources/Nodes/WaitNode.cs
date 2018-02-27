namespace Unscientificlab.BehaviourTree
{
    public class WaitNode<TBlackboard> : BehaviourTreeNode<TBlackboard>
    {
        private readonly ITickProvider _tickProvider;
        private readonly ValueSupplier<TBlackboard, int> _ticksToWaitSupplier;
        private int _startTickId;

        public WaitNode(string name, ITickProvider tickProvider, ValueSupplier<TBlackboard, int> ticksToWaitSupplier) : base(name)
        {
            _tickProvider = tickProvider;
            _ticksToWaitSupplier = ticksToWaitSupplier;
        }

        public override void DeclareNode(BehaviourTreeMetadata<TBlackboard> metadata)
        {
            base.DeclareNode(metadata);
            _startTickId = metadata.DeclareVariable(this, "startTick");
        }

        public override void Initialize(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            data.Variables[_startTickId] = int.MinValue;
        }

        protected override BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            const int undefined = int.MinValue;
            var tick = _tickProvider.GetTick();

            if (data.Variables[_startTickId] == undefined)
                data.Variables[_startTickId] = tick;

            if (tick - data.Variables[_startTickId] < _ticksToWaitSupplier(blackboard))
                return BehaviourTreeStatus.Running;

            data.Variables[_startTickId] = undefined;
            return BehaviourTreeStatus.Success;
        }
    }
}