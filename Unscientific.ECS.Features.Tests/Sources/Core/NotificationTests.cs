using System.Collections.Generic;
using NUnit.Framework;
using Unscientific.ECS.DSL;
using Unscientific.ECS.Features.Core;

namespace Unscientific.ECS.Features.Tests.Sources.Core
{
    [TestFixture]
    public class NotificationTests
    {
        [Test]
        public void GlobalNotificationsTests()
        {
            // @formatter:off
            var world = new WorldBuilder()
                .AddCoreFeature()
                .AddFeature("TestNotifications")
                    .DependsOn()
                        .Feature(CoreFeature.Name)
                    .End()
                    .Components<Game>()
                        .Add<EmptyTestComponent>()
                    .End()
                    .GlobalComponentNotifications<Game>()
                        .AddAllNotifications<EmptyTestComponent>()
                    .End()
                .End()
            .Build();
            // @formatter:on

            var log = new List<string>();

            world.Setup();
            world.Contexts.Singleton().AddComponentListener(new EmptyTestComponentNotificationsListener(log));

            var context = world.Contexts.Get<Game>();
            var entity = context.CreateEntity();

            entity.Add(new EmptyTestComponent());
            entity.Replace(new EmptyTestComponent());
            entity.Remove<EmptyTestComponent>();
            world.Update();
            world.Cleanup();
            world.Clear();

            CollectionAssert.AreEqual(new[] {"Added", "Removed", "Replaced"}, log);
        }

        [Test]
        public void NotificationsTests()
        {
            // @formatter:off
            var world = new WorldBuilder()
                .AddCoreFeature()
                .AddFeature("TestNotifications")
                    .DependsOn()
                        .Feature(CoreFeature.Name)
                    .End()
                    .Components<Game>()
                        .Add<EmptyTestComponent>()
                    .End()
                    .ComponentNotifications<Game>()
                        .AddAllNotifications<EmptyTestComponent>()
                    .End()
                .End()
            .Build();
            // @formatter:on

            var log = new List<string>();
            
            world.Setup();

            var context = world.Contexts.Get<Game>();
            var entity = context.CreateEntity();

            entity.AddComponentListener(new EmptyTestComponentNotificationsListener(log));
            entity.Add(new EmptyTestComponent());
            entity.Replace(new EmptyTestComponent());
            entity.Remove<EmptyTestComponent>();

            world.Update();
            world.Cleanup();
            world.Clear();

            CollectionAssert.AreEqual(new[] {"Added", "Removed", "Replaced"}, log);
        }
        
    }
}