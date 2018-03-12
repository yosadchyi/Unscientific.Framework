using NUnit.Framework;

namespace Unscientific.BehaviourTree.Tests
{
    public class BehaviourTreeTests
    {
        [Test]
        public void ActionTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Do("Increment counter 1", TestActions.IncrementCounter1)
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);
            executor.Tick(data, blackboard);
            var status = data.Statuses[root.Id];

            Assert.AreEqual(BehaviourTreeStatus.Success, status);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void InvertTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Inverter("Invert status")
                    .Do("Increment counter 1", TestActions.IncrementCounter1)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);
            executor.Tick(data, blackboard);
            var status = data.Statuses[root.Id];

            Assert.AreEqual(BehaviourTreeStatus.Failure, status);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void InvertRunningTest()
        {
            var tickProvider = new TestTickProvider();
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Inverter("Invert status")
                    .Wait("Increment counter 1", tickProvider, 3)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);

            tickProvider.Tick();
            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);

            tickProvider.Tick();
            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);

            tickProvider.Tick();
            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Failure, data.Statuses[root.Id]);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void IfNodeFalseTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .If("If false", TestActions.False)
                    .Do("Increment counter 1", TestActions.IncrementCounter1)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);
            executor.Tick(data, blackboard);

            var status = data.Statuses[root.Id];

            Assert.AreEqual(BehaviourTreeStatus.Failure, status);
            Assert.AreEqual(0, blackboard.Counter1);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void IfNodeFailTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .If("If false", TestActions.True)
                    .AlwaysFail("Fail")
                        .Do("Increment counter 1", TestActions.IncrementCounter1)
                    .End()
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);
            executor.Tick(data, blackboard);

            var status = data.Statuses[root.Id];

            Assert.AreEqual(BehaviourTreeStatus.Failure, status);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void IfNodeTrueTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .If("If true", TestActions.True)
                    .Do("Increment counter 1", TestActions.IncrementCounter1)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);
            executor.Tick(data, blackboard);

            var status = data.Statuses[root.Id];

            Assert.AreEqual(BehaviourTreeStatus.Success, status);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void AlwaysFailTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .AlwaysFail("Fail")
                    .Do("Increment counter 1", TestActions.IncrementCounter1)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);
            executor.Tick(data, blackboard);

            var status = data.Statuses[root.Id];

            Assert.AreEqual(BehaviourTreeStatus.Failure, status);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void AlwaysSucceedTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .AllwaysSucceed("Succeed")
                    .AlwaysFail("Fail")
                        .Do("Increment counter 1", TestActions.IncrementCounter1)
                    .End()
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);
            executor.Tick(data, blackboard);

            var status = data.Statuses[root.Id];

            Assert.AreEqual(BehaviourTreeStatus.Success, status);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void WaitNodeTest()
        {
            var tickProvider = new TestTickProvider();
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Wait("Increment counter 1", tickProvider, 3)
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            for (var i = 0; i < 3; i++)
            {
                executor.Tick(data, blackboard);
                Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);
                tickProvider.Tick();
            }

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Success, data.Statuses[root.Id]);
            Assert.AreEqual(0, data.Stack.Count);
        }


        [Test]
        public void ParallelTest()
        {
            var tickProvider = new TestTickProvider();
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Parallel("Parallel")
                    .Do("Increment counter 1", TestActions.IncrementCounter1)
                    .Wait("Wait", tickProvider, 1)
                    .Do("Increment counter 2", TestActions.IncrementCounter2)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(1, blackboard.Counter2);

            tickProvider.Tick();
            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Success, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(1, blackboard.Counter2);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void SequenceRunningTest()
        {
            var tickProvider = new TestTickProvider();
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Sequence("Sequence")
                    .Do("Increment counter 1", TestActions.IncrementCounter1)
                    .Wait("Wait", tickProvider, 1)
                    .Do("Increment counter 2", TestActions.IncrementCounter2)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, blackboard.Counter2);

            tickProvider.Tick();
            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Success, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(1, blackboard.Counter2);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void SequenceFailureTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Sequence("Sequence")
                    .AlwaysFail("Fail")
                        .Do("Increment counter 1", TestActions.IncrementCounter1)
                    .End()
                    .Do("Increment counter 2", TestActions.IncrementCounter2)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Failure, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, blackboard.Counter2);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void SelectorFailureTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Selector("Sequence")
                    .AlwaysFail("Fail")
                        .Do("Increment counter 1", TestActions.IncrementCounter1)
                    .End()
                    .Do("Increment counter 2", TestActions.IncrementCounter2)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Success, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(1, blackboard.Counter2);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void SelectorSuccessTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Selector("Sequence")
                    .Do("Increment counter 1", TestActions.IncrementCounter1)
                    .Do("Increment counter 2", TestActions.IncrementCounter2)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Success, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, blackboard.Counter2);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void SelectorRunningTest()
        {
            var tickProvider = new TestTickProvider();
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Selector("Sequence")
                    .AlwaysFail("Fail increment")
                        .Do("Increment counter 1", TestActions.IncrementCounter1)
                    .End()
                    .Wait("Wait 1 tick", tickProvider, 1)
                    .Do("Increment counter 2", TestActions.IncrementCounter2)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, blackboard.Counter2);

            tickProvider.Tick();
            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Success, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, blackboard.Counter2);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void RepeatWhileStatusReachedNodeTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .WhileFail("Repeat while failure")
                    .Do("Reach limit", TestActions.ReachLimitAction)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);
            Assert.AreEqual(2, blackboard.Counter1);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Success, data.Statuses[root.Id]);
            Assert.AreEqual(3, blackboard.Counter1);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void RepeatUntilStatusReachedNodeTest()
        {
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .UntilSucceess("Repeat until success")
                    .Do("Reach limit", TestActions.ReachLimitAction)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);
            Assert.AreEqual(2, blackboard.Counter1);

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Success, data.Statuses[root.Id]);
            Assert.AreEqual(3, blackboard.Counter1);
            Assert.AreEqual(0, data.Stack.Count);
        }

        [Test]
        public void InterruptionTest()
        {
            var tickProvider = new TestTickProvider();
            var root = new BehaviourTreeBuilder<TestBlackboard>()
                .Sequence("Sequence")
                    .Inverter("Inverter")
                        .Do("Reach limit", TestActions.ReachLimitAction)
                    .End()
                    .Wait("Wait 5 ticks", tickProvider, 5)
                .End()
                .Build();
            var metadata = new BehaviourTreeMetadata<TestBlackboard>(root);
            var data = metadata.CreateExecutionData();
            var executor = new BehaviourTreeExecutor<TestBlackboard>(root);
            var blackboard = new TestBlackboard();

            executor.Start(data);

            for (var i = 0; i < 5; i++)
            {
                executor.Tick(data, blackboard);
                Assert.AreEqual(BehaviourTreeStatus.Running, data.Statuses[root.Id]);
                Assert.AreEqual(1, blackboard.Counter1);
                tickProvider.Tick();
            }

            executor.Tick(data, blackboard);
            Assert.AreEqual(BehaviourTreeStatus.Success, data.Statuses[root.Id]);
            Assert.AreEqual(1, blackboard.Counter1);
            Assert.AreEqual(0, data.Stack.Count);
        }
    }
}