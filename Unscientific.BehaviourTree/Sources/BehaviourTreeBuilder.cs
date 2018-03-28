using System;

namespace Unscientific.BehaviourTree
{
    public interface INodeHandler<TBlackboard>
    {
        void DoHandleNode(BehaviourTreeNode<TBlackboard> node);
    }

    public class BehaviourTreeBuilderLeaf<TBlackboard> : INodeHandler<TBlackboard>
    {
        private readonly BehaviourTreeNode<TBlackboard> _node;

        public BehaviourTreeBuilderLeaf(BehaviourTreeNode<TBlackboard> node)
        {
            _node = node;
        }

        public BehaviourTreeNode<TBlackboard> Build()
        {
            if (_node == null)
                throw new ApplicationException("Can't create tree without node.");
            DoHandleNode(_node);
            return _node;
        }

        #region NodeHandler implementation

        public void DoHandleNode(BehaviourTreeNode<TBlackboard> node)
        {
        }

        #endregion
    }

    public abstract class BehaviourTreeBuilderBase<TBlackboard, TResult, TParent> : INodeHandler<TBlackboard>
        where TParent : INodeHandler<TBlackboard>
    {
        public TResult Do(string name, IAction<TBlackboard> action)
        {
            return HandleNode(new ActionNode<TBlackboard>(name, action.Execute));
        }

        public TResult Do(string name, Func<TBlackboard, BehaviourTreeStatus> actionFn)
        {
            return HandleNode(new ActionNode<TBlackboard>(name, actionFn));
        }

        public TResult Condition(string name, Func<TBlackboard, bool> predicate)
        {
            return HandleNode(new ConditionNode<TBlackboard>(name, predicate));
        }

        public TResult Wait(string name, ITickSupplier tickSupplier, int framesToWait)
        {
            return Wait(name, tickSupplier, new ConstantValueSupplier<TBlackboard, int>(framesToWait));
        }

        public TResult Wait(string name, ITickSupplier tickSupplier, IValueSupplier<TBlackboard, int> framesToWaitSupplier)
        {
            return HandleNode(new WaitNode<TBlackboard>(name, tickSupplier, framesToWaitSupplier));
        }

        public TResult Splice(BehaviourTreeNode<TBlackboard> node)
        {
            return HandleNode(node);
        }

        protected abstract TResult HandleNode(BehaviourTreeNode<TBlackboard> node);

        protected abstract TParent GetThisAsParentFor(BehaviourTreeNode<TBlackboard> node);

        public void DoHandleNode(BehaviourTreeNode<TBlackboard> node)
        {
            HandleNode(node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> Inverter(string name)
        {
            var node = new InverterNode<TBlackboard>(name);

            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> If(string name, Func<TBlackboard, bool> predicate)
        {
            var node = new IfNode<TBlackboard>(name, predicate);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> AlwaysFail(string name)
        {
            var node = new FailerNode<TBlackboard>(name);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> AllwaysSucceed(string name)
        {
            var node = new SucceederNode<TBlackboard>(name);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> WhileSucceess(string name)
        {
            var node = new RepeatWhileStatus<TBlackboard>(name, BehaviourTreeStatus.Success);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> WhileFail(string name)
        {
            var node = new RepeatWhileStatus<TBlackboard>(name, BehaviourTreeStatus.Failure);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> UntilSucceess(string name)
        {
            var node = new RepeatUntilStatusReachedNode<TBlackboard>(name, BehaviourTreeStatus.Success);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
        }

        public BehaviourTreeDecoratorBuilder<TBlackboard, TParent> UntilFail(string name)
        {
            var node = new RepeatUntilStatusReachedNode<TBlackboard>(name, BehaviourTreeStatus.Failure);
            return new BehaviourTreeDecoratorBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
        }

        public BehaviourTreeGroupBuilder<TBlackboard, TParent> Selector(string name)
        {
            var node = new SelectorNode<TBlackboard>(name);
            return new BehaviourTreeGroupBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
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
            return new BehaviourTreeParallelBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
        }

        public BehaviourTreeGroupBuilder<TBlackboard, TParent> Sequence(string name)
        {
            var node = new SequenceNode<TBlackboard>(name);
            return new BehaviourTreeGroupBuilder<TBlackboard, TParent>(GetThisAsParentFor(node), node);
        }
    }

    public interface IBehaviourTreeEnableBuilder<TBlackboard, out TParent> : INodeHandler<TBlackboard>
        where TParent : INodeHandler<TBlackboard>
    {
        TParent End();
    }

    public class BehaviourTreeDecoratorBuilder<TBlackboard, TParent> :
        BehaviourTreeBuilderBase<TBlackboard, IBehaviourTreeEnableBuilder<TBlackboard, TParent>,
            IBehaviourTreeEnableBuilder<TBlackboard, TParent>>,
        IBehaviourTreeEnableBuilder<TBlackboard, TParent>
        where TParent : INodeHandler<TBlackboard>
    {
        private readonly BaseDecoratorNode<TBlackboard> _decorator;
        private readonly TParent _parent;

        public BehaviourTreeDecoratorBuilder(TParent parent, BaseDecoratorNode<TBlackboard> group)
        {
            _parent = parent;
            _decorator = group;
        }

        protected override IBehaviourTreeEnableBuilder<TBlackboard, TParent> HandleNode(
            BehaviourTreeNode<TBlackboard> node)
        {
            _decorator.Node = node;
            return this;
        }

        protected override IBehaviourTreeEnableBuilder<TBlackboard, TParent> GetThisAsParentFor(
            BehaviourTreeNode<TBlackboard> node)
        {
            return this;
        }

        public TParent End()
        {
            _parent.DoHandleNode(_decorator);
            return _parent;
        }
    }

        public class BehaviourTreeGroupBuilder<TBlackboard, TParent> :
            BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeGroupBuilder<TBlackboard, TParent>,
                BehaviourTreeGroupBuilder<TBlackboard, TParent>>,
            IBehaviourTreeEnableBuilder<TBlackboard, TParent>
            where TParent : INodeHandler<TBlackboard>
    {
        private readonly CompositeNode<TBlackboard> _group;
        private readonly TParent _parent;

        public BehaviourTreeGroupBuilder(TParent parent, CompositeNode<TBlackboard> group)
        {
            _parent = parent;
            _group = group;
        }

        protected override BehaviourTreeGroupBuilder<TBlackboard, TParent> HandleNode(
            BehaviourTreeNode<TBlackboard> node)
        {
            _group.AddChild(node);
            return this;
        }

        protected override BehaviourTreeGroupBuilder<TBlackboard, TParent> GetThisAsParentFor(
            BehaviourTreeNode<TBlackboard> node)
        {
            return this;
        }

        public TParent End()
        {
            _parent.DoHandleNode(_group);
            return _parent;
        }
    }

    public class BehaviourTreeParallelBuilder<TBlackboard, TParent> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeParallelBuilder<TBlackboard, TParent>,
            BehaviourTreeParallelBuilder<TBlackboard, TParent>>,
        IBehaviourTreeEnableBuilder<TBlackboard, TParent>
        where TParent : INodeHandler<TBlackboard>
    {
        private readonly ParallelNode<TBlackboard> _group;
        private readonly TParent _parent;

        public BehaviourTreeParallelBuilder(TParent parent, CompositeNode<TBlackboard> group)
        {
            _parent = parent;
            _group = (ParallelNode<TBlackboard>) group;
        }

        public BehaviourTreeParallelBuilder<TBlackboard, TParent> WithFailurePolicy(ParallelPolicy failurePolicy)
        {
            _group.FailurePolicy = failurePolicy;
            return this;
        }

        public BehaviourTreeParallelBuilder<TBlackboard, TParent> WithSucceedPolicy(ParallelPolicy succeedPolicy)
        {
            _group.SucceedPolicy = succeedPolicy;
            return this;
        }

        protected override BehaviourTreeParallelBuilder<TBlackboard, TParent> HandleNode(
            BehaviourTreeNode<TBlackboard> node)
        {
            _group.AddChild(node);
            return this;
        }

        protected override BehaviourTreeParallelBuilder<TBlackboard, TParent> GetThisAsParentFor(
            BehaviourTreeNode<TBlackboard> node)
        {
            return this;
        }

        public TParent End()
        {
            _parent.DoHandleNode(_group);
            return _parent;
        }
    }

    public class BehaviourTreeBuilder<TBlackboard> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeBuilderLeaf<TBlackboard>,
            BehaviourTreeBuilderLeaf<TBlackboard>>
    {
        protected override BehaviourTreeBuilderLeaf<TBlackboard> HandleNode(BehaviourTreeNode<TBlackboard> node)
        {
            return new BehaviourTreeBuilderLeaf<TBlackboard>(node);
        }

        protected override BehaviourTreeBuilderLeaf<TBlackboard> GetThisAsParentFor(BehaviourTreeNode<TBlackboard> node)
        {
            return new BehaviourTreeBuilderLeaf<TBlackboard>(node);
        }
    }
}