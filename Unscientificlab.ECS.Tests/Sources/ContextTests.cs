using System;
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
                    .Add<ValueComponent>()
                    .Add<DeadFlagComponent>()
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
            var entity = _context.CreateEntity().Add(new ValueComponent(12));

            Assert.AreEqual(12, entity.Get<ValueComponent>().Value);
        }

        [Test]
        public void EntityShouldHaveComponentAfterAdding()
        {
            var entity = _context.CreateEntity().Add(new ValueComponent(12));

            Assert.AreEqual(true, entity.Has<ValueComponent>());
        }

        [Test]
        public void EntityShouldNotHaveComponentAfterRemoval()
        {
            var entity = _context.CreateEntity().Add(new ValueComponent(12));

            Assert.AreEqual(true, entity.Has<ValueComponent>());
            entity.Remove<ValueComponent>();
            Assert.AreEqual(false, entity.Has<ValueComponent>());
        }

        [Test]
        public void GetShouldReturnUpdatedComponentAfterReplace()
        {
            var entity = _context.CreateEntity().Add(new ValueComponent(12));

            Assert.AreEqual(12, entity.Get<ValueComponent>().Value);
            entity.Replace(new ValueComponent(122));
            Assert.AreEqual(122, entity.Get<ValueComponent>().Value);
        }

        [Test]
        public void CreateAndDestroyEntityAtBeginningTest()
        {
            var entity1 = _context.CreateEntity().Add(new ValueComponent(123));
            var entity2 = _context.CreateEntity().Add(new ValueComponent(131));
            var entity3 = _context.CreateEntity().Add(new ValueComponent(111));

            _context.DestroyEntity(entity1);

            var enumerator = _context.All().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(111, enumerator.Current.Get<ValueComponent>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(131, enumerator.Current.Get<ValueComponent>().Value);
            enumerator.Dispose();

            Assert.AreEqual(111, _context.GetEntityById(3).Get<ValueComponent>().Value);
            Assert.AreEqual(131, _context.GetEntityById(2).Get<ValueComponent>().Value);
        }

        [Test]
        public void CreateAndDestroyEntityInMiddleTest()
        {
            var entity1 = _context.CreateEntity().Add(new ValueComponent(123));
            var entity2 = _context.CreateEntity().Add(new ValueComponent(131));
            var entity3 = _context.CreateEntity().Add(new ValueComponent(111));

            _context.DestroyEntity(entity2);

            var enumerator = _context.All().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(123, enumerator.Current.Get<ValueComponent>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(111, enumerator.Current.Get<ValueComponent>().Value);
            enumerator.Dispose();

            Assert.AreEqual(123, _context.GetEntityById(1).Get<ValueComponent>().Value);
            Assert.AreEqual(111, _context.GetEntityById(3).Get<ValueComponent>().Value);
        }

        [Test]
        public void DestroyAllEntities()
        {
            _context.CreateEntity().Add(new ValueComponent(123));
            _context.CreateEntity().Add(new ValueComponent(131));
            _context.CreateEntity().Add(new ValueComponent(111));

            _context.DestroyEntity(_context[1]);
            _context.DestroyEntity(_context[2]);
            _context.DestroyEntity(_context[3]);

            var enumerator = _context.All().GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();

            Assert.AreEqual(0, _context.Count);
        }

        [Test]
        public void CreateAndDestroyEntityAtEndTest()
        {
            var entity1 = _context.CreateEntity().Add(new ValueComponent(123));
            var entity2 = _context.CreateEntity().Add(new ValueComponent(131));
            var entity3 = _context.CreateEntity().Add(new ValueComponent(111));

            _context.DestroyEntity(entity3);

            var enumerator = _context.All().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(123, enumerator.Current.Get<ValueComponent>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(131, enumerator.Current.Get<ValueComponent>().Value);
            enumerator.Dispose();

            Assert.AreEqual(123, _context.GetEntityById(1).Get<ValueComponent>().Value);
            Assert.AreEqual(131, _context.GetEntityById(2).Get<ValueComponent>().Value);
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
                entity.Add(new ValueComponent(i + 1));
            }

            var count = 0;

            foreach (var entity in _context.All())
            {
                count++;
                
                Assert.AreEqual(count, entity.Get<ValueComponent>().Value);
            }

            Assert.AreEqual(16, count);
        }

        [Test]
        public void GetAllEntitiesWithDeadFlagShouldReturnAllEntitiesWithDeadFlag()
        {
            for (var i = 1; i <= 16; i++)
            {
                var entity = _context.CreateEntity();
                entity.Add(new ValueComponent(i));
                if (i % 2 == 0)
                    entity.Add(new DeadFlagComponent());
            }

            var count = 0;
            
            foreach (var entity in _context.AllWith<DeadFlagComponent>())
            {
                Assert.IsTrue(entity.Has<DeadFlagComponent>());
                Assert.IsTrue(entity.Get<ValueComponent>().Value % 2 == 0);
                count++;
            }

            Assert.AreEqual(8, count);
        }
        
        [Test]
        public void GetAllEntitiesWithTwoComponents()
        {
            for (var i = 1; i <= 16; i++)
            {
                var entity = _context.CreateEntity();
                entity.Add(new ValueComponent(i));
                if (i % 2 != 0)
                    entity.Add(new DeadFlagComponent());
            }

            var count = 0;
            
            foreach (var entity in _context.AllWith<ValueComponent, DeadFlagComponent>())
            {
                Assert.IsTrue(entity.Has<DeadFlagComponent>());
                Assert.IsTrue(entity.Get<ValueComponent>().Value % 2 != 0);
                count++;
            }

            Assert.AreEqual(8, count);
        }

    }
}
