using NUnit.Framework;

namespace Unscientificlab.ECS.Tests
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
                .Register(_bus);
        }

        [TearDown]
        public void TearDown()
        {
            _bus.Clear();
        }
        
        [Test]
        public void SendMessageTest()
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
        public void SendMessagesTest()
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
        public void ClearShouldRemoveAllMessages()
        {
            var count = 0;
            
            for (var i = 0; i < 16; i++)
                _bus.Send(new TestMessage(i));

            _bus.Clear<TestMessage>();

            foreach (var message in _bus.All<TestMessage>())
            {
                count++;
            }
            
            Assert.AreEqual(0, count);
        }

    }
}