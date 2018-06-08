using System;

namespace Unscientific.BehaviourTree
{
    public class SimpleBehaviourTreeBuilder<TBlackboard>
    {
        private readonly SimpleBehaviourTreeBuilder<TBlackboard> _parentBuilder;
        private readonly BehaviourTreeNode<TBlackboard> _parentNode;
        private BehaviourTreeNode<TBlackboard> _node;

        public SimpleBehaviourTreeBuilder()
        {
        }

        public SimpleBehaviourTreeBuilder(SimpleBehaviourTreeBuilder<TBlackboard> parentBuilder, BehaviourTreeNode<TBlackboard> parentNode)
        {
            _parentBuilder = parentBuilder;
            _parentNode = parentNode;
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> Do(string name, IAction<TBlackboard> action)
        {
            AcceptNode(new ActionNode<TBlackboard>(name, action.Execute));

            return GetBuilderMethodResult();
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> Do(string name, Func<TBlackboard, BehaviourTreeStatus> actionFn)
        {
            AcceptNode(new ActionNode<TBlackboard>(name, actionFn));
            return GetBuilderMethodResult();
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> Condition(string name, Func<TBlackboard, bool> predicate)
        {
            AcceptNode(new ConditionNode<TBlackboard>(name, predicate));
            return GetBuilderMethodResult();
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> Wait(string name, int framesToWait, ITickSupplier tickSupplier)
        {
            return Wait(name, new ConstantValueSupplier<TBlackboard, int>(framesToWait), tickSupplier);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> Wait(string name, IValueSupplier<TBlackboard, int> framesToWaitSupplier, ITickSupplier tickSupplier)
        {
            AcceptNode(new WaitNode<TBlackboard>(name, tickSupplier, framesToWaitSupplier));
            return GetBuilderMethodResult();
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> Splice(BehaviourTreeNode<TBlackboard> node)
        {
            AcceptNode(node);
            return GetBuilderMethodResult();
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> Inverter(string name)
        {
            var node = new InverterNode<TBlackboard>(name);
            AcceptNode(node);

            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> If(string name, Func<TBlackboard, bool> predicate)
        {
            var node = new IfNode<TBlackboard>(name, predicate);
            AcceptNode(node);
            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> AlwaysFail(string name)
        {
            var node = new FailerNode<TBlackboard>(name);
            AcceptNode(node);
            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> AllwaysSucceed(string name)
        {
            var node = new SucceederNode<TBlackboard>(name);
            AcceptNode(node);
            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> WhileSucceess(string name)
        {
            var node = new RepeatWhileStatus<TBlackboard>(name, BehaviourTreeStatus.Success);
            AcceptNode(node);
            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> WhileFail(string name)
        {
            var node = new RepeatWhileStatus<TBlackboard>(name, BehaviourTreeStatus.Failure);
            AcceptNode(node);
            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> UntilSucceess(string name)
        {
            var node = new RepeatUntilStatusReachedNode<TBlackboard>(name, BehaviourTreeStatus.Success);
            AcceptNode(node);
            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> UntilFail(string name)
        {
            var node = new RepeatUntilStatusReachedNode<TBlackboard>(name, BehaviourTreeStatus.Failure);
            AcceptNode(node);
            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> Selector(string name)
        {
            var node = new SelectorNode<TBlackboard>(name);
            AcceptNode(node);
            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> Parallel(string name,
            ParallelPolicy failurePolicy = ParallelPolicy.RequireOne,
            ParallelPolicy succeedPolicy = ParallelPolicy.RequireAll)
        {
            var node = new ParallelNode<TBlackboard>(name)
            {
                FailurePolicy = failurePolicy,
                SucceedPolicy = succeedPolicy
            };
            AcceptNode(node);
            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> Sequence(string name)
        {
            var node = new SequenceNode<TBlackboard>(name);
            AcceptNode(node);
            return new SimpleBehaviourTreeBuilder<TBlackboard>(this, _node);
        }

        private SimpleBehaviourTreeBuilder<TBlackboard> GetBuilderMethodResult()
        {
            return this;
        }

        public SimpleBehaviourTreeBuilder<TBlackboard> End()
        {
            return _parentBuilder;
        }

        public BehaviourTreeNode<TBlackboard> Build()
        {
            return _node;
        }

        private void AcceptNode(BehaviourTreeNode<TBlackboard> node)
        {
            if (_parentNode != null)
            {
                var decorator = _parentNode as BaseDecoratorNode<TBlackboard>;
                var group = _parentNode as CompositeNode<TBlackboard>;

                if (decorator != null)
                {
                    decorator.Node = node;
                }
                else
                {
                    group?.AddChild(node);
                }
            }
            _node = node;
        }
    }
}