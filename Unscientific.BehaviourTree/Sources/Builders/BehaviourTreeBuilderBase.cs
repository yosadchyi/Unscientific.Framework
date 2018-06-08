using System;

namespace Unscientific.BehaviourTree
{
    public abstract class BehaviourTreeBuilderBase<TBlackboard, TBuilderMethodResult>
    {
        public TBuilderMethodResult Do(string name, IAction<TBlackboard> action)
        {
            AcceptNode(new ActionNode<TBlackboard>(name, action.Execute));

            return GetBuilderMethodResult();
        }

        public TBuilderMethodResult Do(string name, Func<TBlackboard, BehaviourTreeStatus> actionFn)
        {
            AcceptNode(new ActionNode<TBlackboard>(name, actionFn));
            return GetBuilderMethodResult();
        }

        public TBuilderMethodResult Condition(string name, Func<TBlackboard, bool> predicate)
        {
            AcceptNode(new ConditionNode<TBlackboard>(name, predicate));
            return GetBuilderMethodResult();
        }

        public TBuilderMethodResult Wait(string name, int framesToWait, ITickSupplier tickSupplier)
        {
            return Wait(name, new ConstantValueSupplier<TBlackboard, int>(framesToWait), tickSupplier);
        }

        public TBuilderMethodResult Wait(string name, IValueSupplier<TBlackboard, int> framesToWaitSupplier, ITickSupplier tickSupplier)
        {
            AcceptNode(new WaitNode<TBlackboard>(name, tickSupplier, framesToWaitSupplier));
            return GetBuilderMethodResult();
        }

        public TBuilderMethodResult Splice(BehaviourTreeNode<TBlackboard> node)
        {
            AcceptNode(node);
            return GetBuilderMethodResult();
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult> Inverter(string name)
        {
            var node = new InverterNode<TBlackboard>(name);
            AcceptNode(node);

            return new BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult> If(string name, Func<TBlackboard, bool> predicate)
        {
            var node = new IfNode<TBlackboard>(name, predicate);
            AcceptNode(node);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult> AlwaysFail(string name)
        {
            var node = new FailerNode<TBlackboard>(name);
            AcceptNode(node);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult> AllwaysSucceed(string name)
        {
            var node = new SucceederNode<TBlackboard>(name);
            AcceptNode(node);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult> WhileSucceess(string name)
        {
            var node = new RepeatWhileStatus<TBlackboard>(name, BehaviourTreeStatus.Success);
            AcceptNode(node);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult> WhileFail(string name)
        {
            var node = new RepeatWhileStatus<TBlackboard>(name, BehaviourTreeStatus.Failure);
            AcceptNode(node);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult> UntilSucceess(string name)
        {
            var node = new RepeatUntilStatusReachedNode<TBlackboard>(name, BehaviourTreeStatus.Success);
            AcceptNode(node);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult> UntilFail(string name)
        {
            var node = new RepeatUntilStatusReachedNode<TBlackboard>(name, BehaviourTreeStatus.Failure);
            AcceptNode(node);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        public BehaviourTreeGroupBuilder<TBlackboard, TBuilderMethodResult> Selector(string name)
        {
            var node = new SelectorNode<TBlackboard>(name);
            AcceptNode(node);
            return new BehaviourTreeGroupBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        public BehaviourTreeParallelBuilder<TBlackboard, TBuilderMethodResult> Parallel(string name,
            ParallelPolicy failurePolicy = ParallelPolicy.RequireOne,
            ParallelPolicy succeedPolicy = ParallelPolicy.RequireAll)
        {
            var node = new ParallelNode<TBlackboard>(name)
            {
                FailurePolicy = failurePolicy,
                SucceedPolicy = succeedPolicy
            };
            AcceptNode(node);
            return new BehaviourTreeParallelBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        public BehaviourTreeGroupBuilder<TBlackboard, TBuilderMethodResult> Sequence(string name)
        {
            var node = new SequenceNode<TBlackboard>(name);
            AcceptNode(node);
            return new BehaviourTreeGroupBuilder<TBlackboard, TBuilderMethodResult>(GetBuilderMethodResult(), node);
        }

        protected abstract TBuilderMethodResult GetBuilderMethodResult();
        protected abstract void AcceptNode(BehaviourTreeNode<TBlackboard> node);
    }
}