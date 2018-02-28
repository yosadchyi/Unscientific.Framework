using System.Collections.Generic;

namespace Unscientificlab.ECS
{
    public class Systems: ISetupSystem, IUpdateSystem, ICleanupSystem
    {
        private readonly List<ISetupSystem> _setupSystems = new List<ISetupSystem>();
        private readonly List<IUpdateSystem> _updateSystems = new List<IUpdateSystem>();
        private readonly List<ICleanupSystem> _cleanupSystems = new List<ICleanupSystem>();
        
        public Systems Add(ISystem system)
        {
            var setupSystem = system as ISetupSystem;
            var updateSystem = system as IUpdateSystem;
            var cleanupSystem = system as ICleanupSystem;

            if (setupSystem != null)
                _setupSystems.Add(setupSystem);

            if (updateSystem != null)
                _updateSystems.Add(updateSystem);

            if (cleanupSystem != null)
                _cleanupSystems.Add(cleanupSystem);

            return this;
        }

        public void Setup()
        {
            foreach (var system in _setupSystems)
                system.Setup();
        }

        public void Update()
        {
            foreach (var system in _updateSystems)
                system.Update();
        }
        
        public void Cleanup()
        {
            foreach (var system in _cleanupSystems)
                system.Cleanup();
        }
    }
}
