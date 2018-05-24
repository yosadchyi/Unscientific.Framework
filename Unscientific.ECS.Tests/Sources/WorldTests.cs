using System.Collections.Generic;
using NUnit.Framework;
using Unscientific.ECS.DSL;

namespace Unscientific.ECS.Tests
{
    [TestFixture]
    public class WorldTests
    {
        [Test]
        public void TestThatSystemsAreExecutedInProperOrder()
        {
            var log = new List<string>();
            // @formatter:off
            var world = new WorldBuilder()
                .AddFeature("Feature 1")
                    .Systems()
                        .Setup(((contexts, bus) => log.Add("F1-S1")))
                        .Update(((contexts, bus) => log.Add("F1-U1")))
                        .Cleanup(((contexts, bus) => log.Add("F1-C1")))
                    .End()
                .End()
                .AddFeature("Feature 2")
                    .Systems()
                        .Setup(((contexts, bus) => log.Add("F2-S1")))
                        .Update(((contexts, bus) => log.Add("F2-U1")))
                        .Cleanup(((contexts, bus) => log.Add("F2-C1")))
                        .Cleanup(((contexts, bus) => log.Add("F2-C2")))
                    .End()
                .End()
                .AddFeature("Feature 3")
                    .Systems()
                        .Setup(((contexts, bus) => log.Add("F3-S1")))
                        .Update(((contexts, bus) => log.Add("F3-U1")))
                        .Cleanup(((contexts, bus) => log.Add("F3-C1")))
                    .End()
                .End()
            .Build();
            // @formatter:on

            world.Setup();
            world.Update();
            world.Cleanup();

            CollectionAssert.AreEqual(new[] {
                "F1-S1", "F2-S1", "F3-S1",
                "F1-U1", "F2-U1", "F3-U1",
                "F3-C1", "F2-C1", "F2-C2", "F1-C1"}, log);
        }
    }
}