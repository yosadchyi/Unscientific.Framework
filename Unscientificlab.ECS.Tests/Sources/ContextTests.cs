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
                .WithComponents()
                    .Add<TestComponent1>()
                .Done()
                .Initialize();
        }

        [TearDown]
        public void Teardown()
        {
            _context.Cleanup();
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

            _context.DestroyEntity(entity1);

            var enumerator = _context.All().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(111, enumerator.Current.Get<TestComponent1>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(131, enumerator.Current.Get<TestComponent1>().Value);
            enumerator.Dispose();
            Assert.Pass();
        }

        [Test]
        public void CreateAndDestroyEntityInMiddleTest()
        {
            var entity1 = _context.CreateEntity().Add(new TestComponent1(123));
            var entity2 = _context.CreateEntity().Add(new TestComponent1(131));
            var entity3 = _context.CreateEntity().Add(new TestComponent1(111));

            _context.DestroyEntity(entity2);

            var enumerator = _context.All().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(123, enumerator.Current.Get<TestComponent1>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(111, enumerator.Current.Get<TestComponent1>().Value);
            enumerator.Dispose();
            Assert.Pass();
        }
        
        [Test]
        public void CreateAndDestroyEntityAtEndTest()
        {
            var entity1 = _context.CreateEntity().Add(new TestComponent1(123));
            var entity2 = _context.CreateEntity().Add(new TestComponent1(131));
            var entity3 = _context.CreateEntity().Add(new TestComponent1(111));

            _context.DestroyEntity(entity3);

            var enumerator = _context.All().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(123, enumerator.Current.Get<TestComponent1>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(131, enumerator.Current.Get<TestComponent1>().Value);
            enumerator.Dispose();
            Assert.Pass();
        }

        [Test]
        public void ContextShouldGrowWhenInitialCapacityReached()
        {
            for (var i = 0; i < 128; i++)
            {
                _context.CreateEntity();
            }
            Assert.Pass();
        }

        [Test]
        public void ContextShouldThrowExceptionWhenMaxCapacityReached()
        {
            TestDelegate testDelegate = () =>
            {
                for (var i = 0; i < 256; i++)
                {
                    _context.CreateEntity();
                }
            };

            Assert.Throws(typeof(ContextReachedMaxCapacityException<TestScope>), testDelegate);
        }

        [Test]
        public void AllShouldReturnAllEntities()
        {
            for (var i = 0; i < 16; i++)
            {
                var entity = _context.CreateEntity();
                entity.Add(new TestComponent1(i + 1));
            }

            var count = 0;
            
            foreach (var entity in _context.All())
            {
                count++;
                
                Assert.AreEqual(count, entity.Get<TestComponent1>().Value);
            }
            
            Assert.AreEqual(16, count);
        }

    }
}
