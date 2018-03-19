using System;
using System.Collections.Generic;
using Unscientific.ECS.Listener;

namespace Unscientific.ECS
{
    public class ModuleUsages
    {
        internal List<Type> Imports { get; } = new List<Type>();

        public ModuleUsages Uses<TModule>()
        {
            Imports.Add(typeof(TModule));
            return this;
        }
    }
    
    public interface IModule
    {
        ModuleUsages Usages();
        ContextRegistrations Contexts();
        MessageRegistrations Messages();
        ComponentRegistrations Components();
        MessageProducerRegistrations MessageProducers();
        Systems Systems(Contexts contexts, MessageBus bus);
    }
}