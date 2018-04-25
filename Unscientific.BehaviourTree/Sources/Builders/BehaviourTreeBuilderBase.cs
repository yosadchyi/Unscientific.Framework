using System;

namespace Unscientific.BehaviourTree
{
    public abstract class BehaviourTreeBuilderBase<TBlackboard, TResult, TParent> : INodeAcceptor<TBlackboard>
        where TParent : INodeAcceptor<TBlackboard>
    {
        public TResult Do(string name, IAction<TBlackboard> action)
        {
            var node = new ActionNode<TBlackboard>(name, action.Execute);

            return ConvertNodeToResult(AcceptNode(node));
        }

        public TResult Do(string name, Func<TBlackboard, BehaviourTreeStatus> actionFn)
        {
            return ConvertNodeToResult(AcceptNode(new ActionNode<TBlackboard>(name, actionFn)));
        }

        public TResult Condition(string name, Func<TBlackboard, bool> predicate)
        {
            return ConvertNodeToResult(AcceptNode(new ConditionNode<TBlackboard>(name, predicate)));
        }

        public TResult Wait(string name, ITickSupplier tickSupplier, int framesToWait)
        {
            return Wait(name, tickSupplier, new ConstantValueSupplier<TBlackboard, int>(framesToWait));
        }

        public TResult Wait(string name, ITickSupplier tickSupplier, IValueSupplier<TBlackboard, int> framesToWaitSupplier)
        {
            return ConvertNodeToResult(AcceptNode(new WaitNode<TBlackboard>(name, tickSupplier, framesToWaitSupplier)));
        }

        public TResult Splice(BehaviourTreeNode<TBlackboard> node)
        {
            return ConvertNodeToResult(node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> Inverter(string name)
        {
            var node = new InverterNode<TBlackboard>(name);

            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> If(string name, Func<TBlackboard, bool> predicate)
        {
            var node = new IfNode<TBlackboard>(name, predicate);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> AlwaysFail(string name)
        {
            var node = new FailerNode<TBlackboard>(name);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> AllwaysSucceed(string name)
        {
            var node = new SucceederNode<TBlackboard>(name);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> WhileSucceess(string name)
        {
            var node = new RepeatWhileStatus<TBlackboard>(name, BehaviourTreeStatus.Success);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> WhileFail(string name)
        {
            var node = new RepeatWhileStatus<TBlackboard>(name, BehaviourTreeStatus.Failure);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> UntilSucceess(string name)
        {
            var node = new RepeatUntilStatusReachedNode<TBlackboard>(name, BehaviourTreeStatus.Success);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> UntilFail(string name)
        {
            var node = new RepeatUntilStatusReachedNode<TBlackboard>(name, BehaviourTreeStatus.Failure);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public BehaviourTreeGroupBuilder<TBlackboard, TParent> Selector(string name)
        {
            var node = new SelectorNode<TBlackboard>(name);
            return new BehaviourTreeGroupBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public BehaviourTreeParallelBuilder<TBlackboard, TParent> Parallel(string name,
            ParallelPolicy failurePolicy = ParallelPolicy.RequireOne,
            ParallelPolicy succeedPolicy = ParallelPolicy.RequireAll)
        {
            var node = new ParallelNode<TBlackboard>(name)
            {
                FailurePolicy = failurePolicy,
                SucceedPolicy = succeedPolicy
            };
            return new BehaviourTreeParallelBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public BehaviourTreeGroupBuilder<TBlackboard, TParent> Sequence(string name)
        {
            var node = new SequenceNode<TBlackboard>(name);
            return new BehaviourTreeGroupBuilder<TBlackboard, TParent>(GetThisForNode(node), node);
        }

        public abstract BehaviourTreeNode<TBlackboard> AcceptNode(BehaviourTreeNode<TBlackboard> node);
        protected abstract TResult ConvertNodeToResult(BehaviourTreeNode<TBlackboard> node);
        protected abstract TParent GetThisForNode(BehaviourTreeNode<TBlackboard> node);
    }
}