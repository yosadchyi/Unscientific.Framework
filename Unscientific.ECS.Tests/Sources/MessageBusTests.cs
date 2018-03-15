using NUnit.Framework;

namespace Unscientific.ECS.Tests
{
    [TestFixture]
    public class MessageBusTests
    {
        private MessageBus _bus;

        [SetUp]
        public void SetUp()
        {
            _bus = new MessageBus();

            new MessageRegistrations()
                .Add<TestMessage>()
                .Add(new KeyMessageAggregator<AggregatedTestMessage, int>(m => m.Value))
                .AddDelayed<DelayedTestMessage>()
                .Register(_bus);
        }

        [TearDown]
        public void TearDown()
        {
            _bus.Cleanup();
        }
        
        [Test]
        public void SendMessageShouldDeliverMessageInCurrentFrame()
        {
            var count = 0;
            
            _bus.Send(new TestMessage(1));

            foreach (var message in _bus.All<TestMessage>())
            {
                Assert.AreEqual(1, message.Value);
                count++;
            }
            
            Assert.AreEqual(1, count);
        }

        [Test]
        public void DelayedMessagesShouldBeDeliveredNextFrame()
        {
            var count = 0;
            
            _bus.Send(new DelayedTestMessage(1));

            foreach (var unused in _bus.All<DelayedTestMessage>())
                count++;

            Assert.AreEqual(0, count);

            _bus.Cleanup();

            count = 0;

            foreach (var message in _bus.All<DelayedTestMessage>())
            {
                Assert.AreEqual(1, message.Value);
                count++;
            }

            Assert.AreEqual(1, count);
        }

        [Test]
        public void SentMessagesShouldBeSameAsReceived()
        {
            var count = 0;
            
            for (var i = 0; i < 16; i++)
                _bus.Send(new TestMessage(i));

            foreach (var message in _bus.All<TestMessage>())
            {
                Assert.AreEqual(count, message.Value);
                count++;
            }
            
            Assert.AreEqual(16, count);
        }

        [Test]
        public void MessagesWithSameKeyShouldBeAggregated()
        {
            var count = 0;
            
            for (var i = 0; i < 16; i++)
                _bus.Send(new AggregatedTestMessage(i / 2));

            foreach (var message in _bus.All<AggregatedTestMessage>())
            {
                Assert.AreEqual(count, message.Value);
                count++;
            }
            
            Assert.AreEqual(8, count);
        }

        [Test]
        public void ClearShouldRemoveAllMessages()
        {
            var count = 0;
            
            for (var i = 0; i < 16; i++)
                _bus.Send(new TestMessage(i));

            _bus.Clear<TestMessage>();

            foreach (var unused in _bus.All<TestMessage>())
                count++;

            Assert.AreEqual(0, count);
        }

    }
}