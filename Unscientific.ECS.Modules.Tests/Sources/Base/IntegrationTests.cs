using NUnit.Framework;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Tests.Base
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.Y, a.Y + b.Y);
        }

        public static Vector2 operator *(Vector2 a, float v)
        {
            return new Vector2(a.X * v, a.Y * v);
        }
    }

    /// <summary>
    /// Sample position component data
    /// </summary>
    public struct Position
    {
        public Vector2 Location;
        public float Rotation;

        public Position(Vector2 location, float rotation)
        {
            Location = location;
            Rotation = rotation;
        }
    }

    /// <summary>
    /// Sample position component data
    /// </summary>
    public struct Velocity
    {
        public Vector2 Linear;
        public float Angular;

        public Velocity(Vector2 linear, float angular)
        {
            Linear = linear;
            Angular = angular;
        }
    }

    public class MoveSystem : IUpdateSystem
    {
        private Context<Game> _context;

        public MoveSystem(Contexts contexts)
        {
            _context = contexts.Get<Game>();
        }

        public void Update()
        {
            foreach (var entity in _context.AllWith<Position, Velocity>())
            {
                var position = entity.Get<Position>();
                var velocity = entity.Get<Velocity>();
                const float dt = 1.0f / 60.0f;

                entity.Replace(new Position(position.Location + velocity.Linear * dt,
                    position.Rotation + velocity.Angular * dt));
            }
        }
    }

    public abstract class MoveModule : IModuleTag
    {
        public class Builder: IModuleBuilder
        {
            public IModule Build()
            {
                return new Module<MoveModule>.Builder()
                        .Usages()
                            .Uses<CoreModule>()
                        .End()
                        .Components<Game>()
                            .Add<Position>()
                            .Add<Velocity>()
                        .End()
                        .Systems()
                            .Add((contexts, bus) => new MoveSystem(contexts))
                        .End()
                    .Build();
            }
        }
    }

    [TestFixture]
    public class IntegrationTests
    {
        private World _world;

        [SetUp]
        public void SetUp()
        {
            _world = new World.Builder()
                    .Using(new CoreModule.Builder().Build())
                    .Using(new MoveModule.Builder().Build())
                .Build();
        }

        [TearDown]
        public void TearDown()
        {
            _world.Clear();
        }

        [Test]
        public void TestUpdateSystem()
        {
            const float eps = 0.0001f;
            var context = _world.Contexts.Get<Game>();

            context.CreateEntity()
                .Add(new Position(new Vector2(0, 1), 0))
                .Add(new Velocity(new Vector2(1, 1), 360));

            _world.Setup();

            for (var i = 0; i < 60; i++)
                _world.Update();

            _world.Cleanup();

            var entity = context.First();
            var position = entity.Get<Position>();

            Assert.AreEqual(1, position.Location.X, eps);
            Assert.AreEqual(2, position.Location.Y, eps);
            Assert.AreEqual(360, position.Rotation, eps);
        }

        [Test]
        public void TestDestroySystem()
        {
            var context = _world.Contexts.Get<Game>();

            _world.Setup();

            context.CreateEntity().Destroy();
            context.CreateEntity().Destroy();
            context.CreateEntity().Destroy();

            Assert.AreEqual(3, context.Count);
            _world.Cleanup();
            Assert.AreEqual(0, context.Count);
        }
    }
}