using System.Collections.Generic;
using NUnit.Framework;

namespace Unscientific.ECS.Tests
{
    [TestFixture]
    public class ApplicationTests
    {
        private static List<IModule> _modules = new List<IModule>();

        private class AddModuleSystem : ISetupSystem
        {
            private readonly IModule _module;

            public AddModuleSystem(IModule module)
            {
                _module = module;
            }
            
            public void Setup()
            {
                _modules.Add(_module);
            }
        }

        private abstract class AbstractModule: IModule
        {
            public virtual ModuleUsages Usages()
            {
                return new ModuleUsages();
            }

            public virtual ContextRegistrations Contexts()
            {
                return new ContextRegistrations();
            }

            public virtual MessageRegistrations Messages()
            {
                return new MessageRegistrations();
            }

            public virtual ComponentRegistrations Components()
            {
                return new ComponentRegistrations();
            }

            public virtual NotificationRegistrations Notifications()
            {
                return new NotificationRegistrations();
            }

            public abstract Systems Systems(Contexts contexts, MessageBus bus);
        }

        private class EmptyModule : AbstractModule
        {
            public override Systems Systems(Contexts contexts, MessageBus bus)
            {
                return new Systems.Builder().Add(new AddModuleSystem(this)).Build();
            }

            public override NotificationRegistrations Notifications()
            {
                return new NotificationRegistrations();
            }
        }

        private class ModuleA : EmptyModule
        {
            public override ModuleUsages Usages()
            {
                return base.Usages()
                    .Uses<ModuleB>()
                    .Uses<ModuleD>();
            }
        }

        private class ModuleB : EmptyModule
        {
            public override ModuleUsages Usages()
            {
                return base.Usages()
                    .Uses<ModuleC>()
                    .Uses<ModuleE>();
            }
        }

        private class ModuleC : EmptyModule
        {
            public override ModuleUsages Usages()
            {
                return base.Usages()
                    .Uses<ModuleD>()
                    .Uses<ModuleE>();
            }
        }

        private class ModuleD : EmptyModule
        {
            private readonly bool _cyclic;

            public ModuleD(bool cyclic)
            {
                _cyclic = cyclic;
            }

            public override ModuleUsages Usages()
            {
                if (!_cyclic)
                    return base.Usages();

                return base.Usages()
                    .Uses<ModuleB>();
            }
        }

        private class ModuleE : EmptyModule
        {
        }

        [Test]
        public void ModuleOrderShouldBeCorrect()
        {
            var moduleA = new ModuleA();
            var moduleB = new ModuleB();
            var moduleC = new ModuleC();
            var moduleD = new ModuleD(false);
            var moduleE = new ModuleE();

            var application = new Game.Builder()
                .Using(moduleA)
                .Using(moduleB)
                .Using(moduleC)
                .Using(moduleD)
                .Using(moduleE)
                .Build();
            
            application.Setup();
            
            Assert.AreEqual(typeof(ModuleE), _modules[0].GetType());
            Assert.AreEqual(typeof(ModuleD), _modules[1].GetType());
            Assert.AreEqual(typeof(ModuleC), _modules[2].GetType());
            Assert.AreEqual(typeof(ModuleB), _modules[3].GetType());
            Assert.AreEqual(typeof(ModuleA), _modules[4].GetType());
        }

        [Test]
        public void CyclicDependencyShouldBeReported()
        {
            var moduleA = new ModuleA();
            var moduleB = new ModuleB();
            var moduleC = new ModuleC();
            var moduleD = new ModuleD(true);
            var moduleE = new ModuleE();

            TestDelegate testDelegate = () => 
                new Game.Builder()
                    .Using(moduleA)
                    .Using(moduleB)
                    .Using(moduleC)
                    .Using(moduleD)
                    .Using(moduleE)
                    .Build();

            Assert.Throws(typeof(ModulesHaveCircularReferenceException), testDelegate);
        }

        [Test]
        public void MissingModuleShouldBeReported()
        {
            var moduleA = new ModuleA();

            TestDelegate testDelegate = () => 
                new Game.Builder()
                    .Using(moduleA)
                    .Build();

            Assert.Throws(typeof(NoRequiredModuleException), testDelegate);
        }
    }
}