using System;
using NUnit.Framework;
using Unscientificlab.ECS;

namespace Unscientificlab.ECS.Tests
{
    /// <summary>
    /// Context and Entity tests
    /// </summary>
    [TestFixture]
    public class ContextTests
    {
        private readonly Contexts _contexts = new Contexts();

        [SetUp]
        public void Setup()
        {
            new Components<TestScope>()
                .Add<ValueComponent>()
                .Add<DeadFlagComponent>()
                .Register();

            new Context<TestScope>.Initializer()
                .WithInitialCapacity(16)
                .WithMaxCapacity(128)
                .WithReferenceTrackerFactory((capacity) => new SafeReferenceTracker<TestScope>(capacity))
                .Initialize();
        }

        [TearDown]
        public void Teardown()
        {
            _contexts.Get<TestScope>().Cleanup();
        }

        [Test]
        public void GetShouldReturnComponent()
        {
            var entity = _contexts.Get<TestScope>().CreateEntity().Add(new ValueComponent(12));

            Assert.AreEqual(12, entity.Get<ValueComponent>().Value);
        }

        [Test]
        public void EntityShouldHaveComponentAfterAdding()
        {
            var entity = _contexts.Get<TestScope>().CreateEntity().Add(new ValueComponent(12));

            Assert.AreEqual(true, entity.Has<ValueComponent>());
        }

        [Test]
        public void EntityShouldNotHaveComponentAfterRemoval()
        {
            var entity = _contexts.Get<TestScope>().CreateEntity().Add(new ValueComponent(12));

            Assert.AreEqual(true, entity.Has<ValueComponent>());
            entity.Remove<ValueComponent>();
            Assert.AreEqual(false, entity.Has<ValueComponent>());
        }

        [Test]
        public void GetShouldReturnUpdatedComponentAfterReplace()
        {
            var entity = _contexts.Get<TestScope>().CreateEntity().Add(new ValueComponent(12));

            Assert.AreEqual(12, entity.Get<ValueComponent>().Value);
            entity.Replace(new ValueComponent(122));
            Assert.AreEqual(122, entity.Get<ValueComponent>().Value);
        }

        [Test]
        public void CreateAndDestroyEntityAtBeginningTest()
        {
            var context = _contexts.Get<TestScope>();
            var entity1 = context.CreateEntity().Add(new ValueComponent(123));
            var entity2 = context.CreateEntity().Add(new ValueComponent(131));
            var entity3 = context.CreateEntity().Add(new ValueComponent(111));

            context.DestroyEntity(entity1);

            var enumerator = context.All().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(111, enumerator.Current.Get<ValueComponent>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(131, enumerator.Current.Get<ValueComponent>().Value);
            enumerator.Dispose();

            Assert.AreEqual(111, context.GetEntityById(3).Get<ValueComponent>().Value);
            Assert.AreEqual(131, context.GetEntityById(2).Get<ValueComponent>().Value);
        }

        [Test]
        public void CreateAndDestroyEntityInMiddleTest()
        {
            var context = _contexts.Get<TestScope>();
            var entity1 = context.CreateEntity().Add(new ValueComponent(123));
            var entity2 = context.CreateEntity().Add(new ValueComponent(131));
            var entity3 = context.CreateEntity().Add(new ValueComponent(111));

            context.DestroyEntity(entity2);

            var enumerator = context.All().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(123, enumerator.Current.Get<ValueComponent>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(111, enumerator.Current.Get<ValueComponent>().Value);
            enumerator.Dispose();

            Assert.AreEqual(123, context.GetEntityById(1).Get<ValueComponent>().Value);
            Assert.AreEqual(111, context.GetEntityById(3).Get<ValueComponent>().Value);
        }

        [Test]
        public void DestroyAllEntities()
        {
            var context = _contexts.Get<TestScope>();
            context.CreateEntity().Add(new ValueComponent(123));
            context.CreateEntity().Add(new ValueComponent(131));
            context.CreateEntity().Add(new ValueComponent(111));

            context.DestroyEntity(context[1]);
            context.DestroyEntity(context[2]);
            context.DestroyEntity(context[3]);

            var enumerator = context.All().GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();

            Assert.AreEqual(0, context.Count);
        }

        [Test]
        public void CreateAndDestroyEntityAtEndTest()
        {
            var context = _contexts.Get<TestScope>();
            var entity1 = context.CreateEntity().Add(new ValueComponent(123));
            var entity2 = context.CreateEntity().Add(new ValueComponent(131));
            var entity3 = context.CreateEntity().Add(new ValueComponent(111));

            context.DestroyEntity(entity3);

            var enumerator = context.All().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(123, enumerator.Current.Get<ValueComponent>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(131, enumerator.Current.Get<ValueComponent>().Value);
            enumerator.Dispose();

            Assert.AreEqual(123, context.GetEntityById(1).Get<ValueComponent>().Value);
            Assert.AreEqual(131, context.GetEntityById(2).Get<ValueComponent>().Value);
        }

        [Test]
        public void ContextShouldGrowWhenInitialCapacityReached()
        {
            var context = _contexts.Get<TestScope>();

            for (var i = 0; i < 128; i++)
            {
                context.CreateEntity();
            }
            Assert.Pass();
        }

        [Test]
        public void ContextShouldThrowExceptionWhenMaxCapacityReached()
        {
            TestDelegate testDelegate = () =>
            {
                var context = _contexts.Get<TestScope>();

                for (var i = 0; i < 256; i++)
                {
                    context.CreateEntity();
                }
            };

            Assert.Throws(typeof(ContextReachedMaxCapacityException<TestScope>), testDelegate);
        }

        [Test]
        public void AllShouldReturnAllEntities()
        {
            var context = _contexts.Get<TestScope>();

            for (var i = 0; i < 16; i++)
            {
                var entity = context.CreateEntity();
                entity.Add(new ValueComponent(i + 1));
            }

            var count = 0;

            foreach (var entity in context.All())
            {
                count++;
                
                Assert.AreEqual(count, entity.Get<ValueComponent>().Value);
            }

            Assert.AreEqual(16, count);
        }

        [Test]
        public void GetAllEntitiesWithDeadFlagShouldReturnAllEntitiesWithDeadFlag()
        {
            var context = _contexts.Get<TestScope>();

            for (var i = 1; i <= 16; i++)
            {
                var entity = context.CreateEntity();
                entity.Add(new ValueComponent(i));
                if (i % 2 == 0)
                    entity.Add(new DeadFlagComponent());
            }

            var count = 0;
            
            foreach (var entity in context.AllWith<DeadFlagComponent>())
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
            var context = _contexts.Get<TestScope>();

            for (var i = 1; i <= 16; i++)
            {
                var entity = context.CreateEntity();
                entity.Add(new ValueComponent(i));
                if (i % 2 != 0)
                    entity.Add(new DeadFlagComponent());
            }

            var count = 0;
            
            foreach (var entity in context.AllWith<ValueComponent, DeadFlagComponent>())
            {
                Assert.IsTrue(entity.Has<DeadFlagComponent>());
                Assert.IsTrue(entity.Get<ValueComponent>().Value % 2 != 0);
                count++;
            }

            Assert.AreEqual(8, count);
        }

        [Test]
        public void RetainedEntityCanNotBeDestroyed()
        {
            var context = _contexts.Get<TestScope>();
            var entity = context.CreateEntity();
            var entityRef = entity.Retain(this);
            TestDelegate testDelegate = () =>
            {
                context.DestroyEntity(entityRef.Entity);
            };

            Assert.Throws(typeof(TryingToDestroyReferencedEntity<TestScope>), testDelegate);
        }

        [Test]
        public void RetainedEntityCanNotBeReleasedByAnotherOwner()
        {
            var context = _contexts.Get<TestScope>();
            var entity = context.CreateEntity();
            var entityRef = entity.Retain(this);
            TestDelegate testDelegate = () =>
            {
                entityRef.Release("anotherObject");
            };

            Assert.Throws(typeof(ReleasingNonOwnedEntityException<TestScope>), testDelegate);
        }

        [Test]
        public void RetainedAndReleasedEntityCanBeDestroyed()
        {
            var context = _contexts.Get<TestScope>();
            var entity = context.CreateEntity();
            var entityRef = entity.Retain(this);
            entityRef.Release(this);
            context.DestroyEntity(entityRef.Entity);
            Assert.Pass();
        }

    }
}
