using System.Collections.Generic;

namespace Unscientific.ECS.DSL
{
    internal class SystemsElement
    {
        internal readonly List<SystemElement> SetupSystems = new List<SystemElement>();
        internal readonly List<SystemElement> UpdateSystems = new List<SystemElement>();
        internal readonly List<SystemElement> CleanupSystems = new List<SystemElement>();

        internal void Add(List<SystemElement> setupSystems, List<SystemElement> updateSystems, List<SystemElement> cleanupSystems)
        {
            SetupSystems.AddRange(setupSystems);
            UpdateSystems.AddRange(updateSystems);
            CleanupSystems.AddRange(cleanupSystems);
        }
        
        internal void Add(SystemsElement element)
        {
            SetupSystems.AddRange(element.SetupSystems);
            UpdateSystems.AddRange(element.UpdateSystems);
            CleanupSystems.AddRange(element.CleanupSystems);
        }
    }
}