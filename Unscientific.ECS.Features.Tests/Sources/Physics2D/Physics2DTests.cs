using NUnit.Framework;
using Unscientific.ECS.DSL;
using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.ECS.Features.Physics2D.Shapes;
using Unscientific.ECS.Features.Tick;
using Unscientific.FixedPoint;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Features.Tests.Sources.Physics2D
{
    [TestFixture]
    public class Physics2DTests
    {
        private static readonly Fix TimeStep = Fix.Ratio(1, 10);

        private World _world;
        private Context<Game> _context;

        [SetUp]
        public void SetUp()
        {
            // @formatter:off
            _world = new WorldBuilder()
                .AddCoreFeature()
                .AddTickFeature()
                .AddPhysics2DFeature(c => c
                    .SetTimeStep(TimeStep)
                    .AddSpatialDatabase(new SpatialHash(Fix.Ratio(1, 2), 133)))
                .Build();
            // @formatter:on
            _context = _world.Contexts.Get<Game>();
        }

        [TearDown]
        public void TearDown()
        {
            _world.Clear();
        }

        [Test]
        public void TestThatTwoCircleShapesAreColliding()
        {
            _world.Setup();

            var position1 = new FixVec2(-1, 0);
            var velocity1 = new FixVec2(1, 0);
            var entity1 = _context.CreateEntity()
                .Add(new Position(position1))
                .Add(new Velocity(velocity1))
                .Add(CreateSimpleCircleBoundingShapes())
                .Add(new Collisions(ListPool<Collision>.Instance.Get()));

            var position2 = new FixVec2(1, 0);
            var velocity2 = new FixVec2(-1, 0);
            var entity2 = _context.CreateEntity()
                .Add(new Position(position2))
                .Add(new Velocity(velocity2))
                .Add(CreateSimpleCircleBoundingShapes())
                .Add(new Collisions(ListPool<Collision>.Instance.Get()));


            var distToGo = Fix.Ratio(50, 100);
            var time = distToGo / PhysicsLimits.Velocity;

            while (time > 0)
            {
                _world.Cleanup();
                _world.Update();
                time -= TimeStep;
            }

            var shapes1 = entity1.Get<BoundingShapes>().Shapes;
            var shapes2 = entity2.Get<BoundingShapes>().Shapes;
            var collisions1 = entity1.Get<Collisions>().List;
            var collisions2 = entity2.Get<Collisions>().List;

            Assert.AreEqual(1, collisions1.Count);
            Assert.AreEqual(1, collisions2.Count);

            var collision1 = collisions1[0];
            var collision2 = collisions2[0];
            
            Assert.AreEqual(shapes1.First, collision1.SelfShape);
            Assert.AreEqual(shapes2.First, collision1.OtherShape);
            Assert.AreEqual(entity2.Id, collision1.Other.Id);

            Assert.AreEqual(shapes2.First, collision2.SelfShape);
            Assert.AreEqual(shapes1.First, collision2.OtherShape);
            Assert.AreEqual(entity1.Id, collision2.Other.Id);
        }

        [Test]
        public void TestThatSensorCollidesWithShape()
        {
            _world.Setup();

            var position1 = new FixVec2(Fix.Ratio(-25, 10), 0);
            var velocity1 = new FixVec2(1, 0);
            var entity1 = _context.CreateEntity()
                .Add(new Position(position1))
                .Add(new Velocity(velocity1))
                .Add(CreateCircleBoundingShapesWithSensor())
                .Add(new Collisions(ListPool<Collision>.Instance.Get()));

            var position2 = new FixVec2(Fix.Ratio(25, 10), 0);
            var velocity2 = new FixVec2(-1, 0);
            var entity2 = _context.CreateEntity()
                .Add(new Position(position2))
                .Add(new Velocity(velocity2))
                .Add(CreateCircleBoundingShapesWithSensor())
                .Add(new Collisions(ListPool<Collision>.Instance.Get()));


            var distToGo = Fix.Ratio(100, 100);
            var time = distToGo / PhysicsLimits.Velocity;

            while (time > 0)
            {
                _world.Cleanup();
                _world.Update();
                time -= TimeStep;
            }

            var collisions1 = entity1.Get<Collisions>().List;
            var collisions2 = entity2.Get<Collisions>().List;

            Assert.AreEqual(1, collisions1.Count);
            Assert.AreEqual(1, collisions2.Count);

            var collision1 = collisions1[0];
            var collision2 = collisions2[0];
            
            Assert.AreEqual(ShapeTags.TargetSensor, collision1.SelfShape.Tag);
            Assert.AreEqual(ShapeTags.BodyShape, collision1.OtherShape.Tag);
            Assert.AreEqual(entity2.Id, collision1.Other.Id);

            Assert.AreEqual(ShapeTags.TargetSensor, collision2.SelfShape.Tag);
            Assert.AreEqual(ShapeTags.BodyShape, collision2.OtherShape.Tag);
            Assert.AreEqual(entity1.Id, collision2.Other.Id);
        }

        private static BoundingShapes CreateSimpleCircleBoundingShapes()
        {
            return new BoundingShapes(ShapeList.New()
                .AddCircle(c =>
                {
                    c.Radius = 1;
                    c.Tag = ShapeTags.BodyShape;
                }));
        }
        
        private static BoundingShapes CreateCircleBoundingShapesWithSensor()
        {
            return new BoundingShapes(ShapeList.New()
                .AddCircle(c =>
                {
                    c.Radius = 1;
                    c.Tag = ShapeTags.BodyShape;
                })
                .AddCircle(c =>
                {
                    c.Sensor = true;
                    c.Radius = 2;
                    c.Tag = ShapeTags.TargetSensor;
                }));
        }
    }

    public static class ShapeTags
    {
        public const string TargetSensor = "TargetSensor";
        public const string BodyShape = "BodyShape";
    }
    
    public static class PhysicsLimits
    {
        public static readonly Fix Velocity = 1;
    
        public static readonly Fix AngularVelocity = FixMath.Pi;
    }

}