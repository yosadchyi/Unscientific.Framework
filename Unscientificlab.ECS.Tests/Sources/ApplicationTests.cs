using System.Collections.Generic;
using NUnit.Framework;

namespace Unscientificlab.ECS.Tests
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
        
        private class EmptyModule : AbstractModule
        {
            public override Systems Systems(Contexts contexts, MessageBus bus)
            {
                return new Systems.Builder().Add(new AddModuleSystem(this)).Build();
            }
        }

        private class ModuleA : EmptyModule
        {
            public override ModuleImports Imports()
            {
                return base.Imports()
                    .Import<ModuleB>()
                    .Import<ModuleD>();
            }
        }

        private class ModuleB : EmptyModule
        {
            public override ModuleImports Imports()
            {
                return base.Imports()
                    .Import<ModuleC>()
                    .Import<ModuleE>();
            }
        }

        private class ModuleC : EmptyModule
        {
            public override ModuleImports Imports()
            {
                return base.Imports()
                    .Import<ModuleD>()
                    .Import<ModuleE>();
            }
        }

        private class ModuleD : EmptyModule
        {
            private readonly bool _cyclic;

            public ModuleD(bool cyclic)
            {
                _cyclic = cyclic;
            }

            public override ModuleImports Imports()
            {
                if (!_cyclic)
                    return base.Imports();

                return base.Imports()
                    .Import<ModuleB>();
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

            var application = new Application.Builder()
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
                new Application.Builder()
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
                new Application.Builder()
                    .Using(moduleA)
                    .Build();

            Assert.Throws(typeof(NoRequiredModuleException), testDelegate);
        }
    }
}