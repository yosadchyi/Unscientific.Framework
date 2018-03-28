using System;
using System.Collections.Generic;

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
}