using System.Collections.Generic;

namespace Unscientific.ECS
{
    public class Systems: ISetupSystem, IUpdateSystem, ICleanupSystem
    {
        public readonly List<ISetupSystem> _setupSystems;
        private readonly List<IUpdateSystem> _updateSystems;
        private readonly List<ICleanupSystem> _cleanupSystems;

        public class Builder
        {
            private readonly List<ISetupSystem> _setupSystems = new List<ISetupSystem>();
            private readonly List<IUpdateSystem> _updateSystems = new List<IUpdateSystem>();
            private readonly List<ICleanupSystem> _cleanupSystems = new List<ICleanupSystem>();

            public Builder AddAll(params ISystem[] systems)
            {
                foreach (var system in systems)
                {
                    Add(system);
                }

                return this;
            }

            public Builder Add(Systems systems)
            {
                _setupSystems.AddRange(systems._setupSystems);
                _updateSystems.AddRange(systems._updateSystems);
                _cleanupSystems.AddRange(systems._cleanupSystems);
                return this;
            }
            
            public Builder Add(ISystem system)
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

            internal Builder ReverseCleanupSystemsOrder()
            {
                _cleanupSystems.Reverse();
                return this;
            }

            public Systems Build()
            {
                return new Systems(_setupSystems, _updateSystems, _cleanupSystems);
            }
        }

        private Systems(List<ISetupSystem> setupSystems, List<IUpdateSystem> updateSystems, List<ICleanupSystem> cleanupSystems)
        {
            _setupSystems = setupSystems;
            _updateSystems = updateSystems;
            _cleanupSystems = cleanupSystems;
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
