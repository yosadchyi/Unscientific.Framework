using NUnit.Framework;

namespace Unscientific.ECS.Tests
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
            new ComponentRegistrations()
                .For<TestScope>()
                    .Add<ValueComponent>()
                    .Add<DeadFlagComponent>()
                .End()
                .Register();

            new Context<TestScope>.Initializer()
                .WithInitialCapacity(16)
                .Initialize();
        }

        [TearDown]
        public void Teardown()
        {
            _contexts.Get<TestScope>().Clear();
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

            var enumerator = context.AllWith<ValueComponent>().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(111, enumerator.Current.Get<ValueComponent>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(131, enumerator.Current.Get<ValueComponent>().Value);
            enumerator.Dispose();

            Assert.AreEqual(111, entity3.Get<ValueComponent>().Value);
            Assert.AreEqual(131, entity2.Get<ValueComponent>().Value);
        }

        [Test]
        public void CreateAndDestroyEntityInMiddleTest()
        {
            var context = _contexts.Get<TestScope>();
            var entity1 = context.CreateEntity().Add(new ValueComponent(123));
            var entity2 = context.CreateEntity().Add(new ValueComponent(131));
            var entity3 = context.CreateEntity().Add(new ValueComponent(111));

            context.DestroyEntity(entity2);

            var enumerator = context.AllWith<ValueComponent>().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(123, enumerator.Current.Get<ValueComponent>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(111, enumerator.Current.Get<ValueComponent>().Value);
            enumerator.Dispose();

            Assert.AreEqual(123, entity1.Get<ValueComponent>().Value);
            Assert.AreEqual(111, entity3.Get<ValueComponent>().Value);
        }

        [Test]
        public void DestroyAllEntities()
        {
            var context = _contexts.Get<TestScope>();
            var entity1 = context.CreateEntity().Add(new ValueComponent(123));
            var entity2 = context.CreateEntity().Add(new ValueComponent(131));
            var entity3 = context.CreateEntity().Add(new ValueComponent(111));

            context.DestroyEntity(entity1);
            context.DestroyEntity(entity2);
            context.DestroyEntity(entity3);

            var enumerator = context.AllWith<ValueComponent>().GetEnumerator();
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

            var enumerator = context.AllWith<ValueComponent>().GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(123, enumerator.Current.Get<ValueComponent>().Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(131, enumerator.Current.Get<ValueComponent>().Value);
            enumerator.Dispose();

            Assert.AreEqual(123, entity1.Get<ValueComponent>().Value);
            Assert.AreEqual(131, entity2.Get<ValueComponent>().Value);
        }

        [Test]
        public void CreateAfterDestroyEntityTest()
        {
            var context = _contexts.Get<TestScope>();
            var entity1 = context.CreateEntity().Add(new ValueComponent(123));
            var entity2 = context.CreateEntity().Add(new ValueComponent(131));

            context.DestroyEntity(entity1);
            entity1 = context.CreateEntity().Add(new ValueComponent(1231));
            context.DestroyEntity(entity2);
            entity2 = context.CreateEntity().Add(new ValueComponent(1311));
            Assert.Pass();
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
        public void AllShouldReturnAllEntities()
        {
            var context = _contexts.Get<TestScope>();

            for (var i = 0; i < 16; i++)
            {
                var entity = context.CreateEntity();
                entity.Add(new ValueComponent(i + 1));
            }

            var count = 0;

            foreach (var entity in context.AllWith<ValueComponent>())
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
    }
}
