using NUnit.Framework;
using Unscientificlab.ECS.Exception;
using Unscientificlab.ECS.ReferenceTracking;

namespace Unscientificlab.ECS.Tests
{
    /// <summary>
    /// Context and Entity tests
    /// </summary>
    [TestFixture]
    public class ContextTests
    {
        private readonly ReferenceTrackerFactory _trackerFactory = (max) => new SafeReferenceTracker<TestScope>();
        private Context<TestScope> _context;

        [SetUp]
        public void Setup()
        {
            _context = new Context<TestScope>.Initializer()
                .WithInitialCapacity(16)
                .WithMaxCapacity(128)
                .WithReferenceTrackerFactory(_trackerFactory)
                .WithComponent<TestComponent1>()
                .Initialize();
        }

        [TearDown]
        public void Teardown()
        {
            _context.Destroy();
        }

        [Test]
        public void EntityShouldNotExistsAfterDestroy()
        {
            var entity = _context.CreateEntity().Add(new TestComponent1(123));

            Assert.AreEqual(0, entity.Id);

            _context.DestroyEntity(entity);
            
            TestDelegate getComponent = () =>
            {
                 _context.Get<TestComponent1>(entity);
            };

            Assert.Throws(typeof(EntityDoesNotExistsException<TestScope>), getComponent);
        }

        [Test]
        public void GetShouldReturnComponent()
        {
            var entity = _context.CreateEntity().Add(new TestComponent1(12));

            Assert.AreEqual(12, entity.Get<TestComponent1>().Value);
        }

        [Test]
        public void EntityShouldHaveComponentAfterAdding()
        {
            var entity = _context.CreateEntity().Add(new TestComponent1(12));

            Assert.AreEqual(true, entity.Has<TestComponent1>());
        }

        [Test]
        public void EntityShouldNotHaveComponentAfterRemoval()
        {
            var entity = _context.CreateEntity().Add(new TestComponent1(12));

            Assert.AreEqual(true, entity.Has<TestComponent1>());
            entity.Remove<TestComponent1>();
            Assert.AreEqual(false, entity.Has<TestComponent1>());
        }

        [Test]
        public void GetShouldReturnUpdatedComponentAfterReplace()
        {
            var entity = _context.CreateEntity().Add(new TestComponent1(12));

            Assert.AreEqual(12, entity.Get<TestComponent1>().Value);
            entity.Replace(new TestComponent1(122));
            Assert.AreEqual(122, entity.Get<TestComponent1>().Value);
        }

        [Test]
        public void CreateAndDestroyEntityAtBeginningTest()
        {
            var entity1 = _context.CreateEntity().Add(new TestComponent1(123));
            var entity2 = _context.CreateEntity().Add(new TestComponent1(131));
            var entity3 = _context.CreateEntity().Add(new TestComponent1(111));
            
            Assert.AreEqual(0, entity1.Id);
            Assert.AreEqual(1, entity2.Id);
            Assert.AreEqual(2, entity3.Id);

            _context.DestroyEntity(entity1);
            
            Assert.AreEqual(131, entity2.Get<TestComponent1>().Value);
            Assert.AreEqual(111, entity3.Get<TestComponent1>().Value);
        }

        [Test]
        public void CreateAndDestroyEntityInMiddleTest()
        {
            var entity1 = _context.CreateEntity().Add(new TestComponent1(123));
            var entity2 = _context.CreateEntity().Add(new TestComponent1(131));
            var entity3 = _context.CreateEntity().Add(new TestComponent1(111));

            Assert.AreEqual(0, entity1.Id);
            Assert.AreEqual(1, entity2.Id);
            Assert.AreEqual(2, entity3.Id);

            _context.DestroyEntity(entity2);
            
            Assert.AreEqual(123, entity1.Get<TestComponent1>().Value);
            Assert.AreEqual(111, entity3.Get<TestComponent1>().Value);
        }
        
        [Test]
        public void CreateAndDestroyEntityAtEndTest()
        {
            var entity1 = _context.CreateEntity().Add(new TestComponent1(123));
            var entity2 = _context.CreateEntity().Add(new TestComponent1(131));
            var entity3 = _context.CreateEntity().Add(new TestComponent1(111));

            Assert.AreEqual(0, entity1.Id);
            Assert.AreEqual(1, entity2.Id);
            Assert.AreEqual(2, entity3.Id);

            _context.DestroyEntity(entity3);

            Assert.AreEqual(123, entity1.Get<TestComponent1>().Value);
            Assert.AreEqual(131, entity2.Get<TestComponent1>().Value);
        }

        [Test]
        public void ContextShouldGrowWhenInitialCapacityReached()
        {
            for (var i = 0; i < 128; i++)
            {
                var entity = _context.CreateEntity();

                Assert.AreEqual(i, entity.Id);
            }
        }

        [Test]
        public void ContextShouldThrowExceptionWhenMaxCapacityReached()
        {
            TestDelegate testDelegate = () =>
            {
                for (var i = 0; i < 256; i++)
                {
                    var entity = _context.CreateEntity();

                    Assert.AreEqual(i, entity.Id);
                }
            };

            Assert.Throws(typeof(ContextReachedMaxCapacityException<TestScope>), testDelegate);
        }

        [Test]
        public void AllShouldReturnAllEntities()
        {
            for (var i = 0; i < 16; i++)
            {
                _context.CreateEntity();
            }

            var count = 0;
            
            foreach (var unused in _context.All())
            {
                count++;
            }
            
            Assert.AreEqual(16, count);
        }

        [Test]
        public void DestroingRetainedEntityShouldThrowException()
        {
            var entity = _context.CreateEntity().Retain(this);

            Assert.Throws(typeof(ReleasingRetainedEntityException<TestScope>), () => _context.DestroyEntity(entity));
        }
    }
}
