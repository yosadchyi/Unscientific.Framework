using System.Collections.Generic;

namespace Unscientificlab.ECS.System
{
    public class ExecuteSystems: IExecuteSystem
    {
        private readonly List<IExecuteSystem> _systems = new List<IExecuteSystem>();
        
        public ExecuteSystems Add(IExecuteSystem system)
        {
            _systems.Add(system);

            return this;
        }

        public void Execute()
        {
            foreach (var system in _systems)
            {
                system.Execute();
            }
        }
    }
}
