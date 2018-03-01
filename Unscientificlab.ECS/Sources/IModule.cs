using System;
using System.Collections.Generic;

namespace Unscientificlab.ECS
{
    public class ModuleImports
    {
        internal List<Type> Imports { get; } = new List<Type>();

        public ModuleImports Import<TModule>() where TModule: IModule
        {
            Imports.Add(typeof(TModule));
            return this;
        }
    }
    
    public interface IModule
    {
        ModuleImports Imports();
        ContextRegistrations Contexts();
        MessageRegistrations Messages();
        ComponentRegistrations Components();
        Systems Systems(Contexts contexts, MessageBus bus);
    }
}