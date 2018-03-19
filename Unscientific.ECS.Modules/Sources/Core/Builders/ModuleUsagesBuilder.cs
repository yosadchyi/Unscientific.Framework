using System;

namespace Unscientific.ECS.Modules.Core
{
    public partial class Module<TTag>
    {
        public partial class Builder
        {
            public class ModuleUsagesBuilder
            {
                private readonly Builder _builder;

                internal ModuleUsagesBuilder(Builder builder)
                {
                    _builder = builder;
                }

                public ModuleUsagesBuilder Uses<TModule>()
                {
                    var iModuleType = typeof(IModule);
                    var iModuleTagType = typeof(IModuleTag);
                    var type = typeof(TModule);

                    if (iModuleTagType.IsAssignableFrom(type))
                    {
                        _builder._usages.Uses<Module<TModule>>();
                    }
                    else if (iModuleType.IsAssignableFrom(type))
                    {
                        _builder._usages.Uses<TModule>();
                    }
                    else
                    {
                        throw new ArgumentException("Uses can accept only types conforming to IModule or IModuleTag interfaces!");
                    }

                    return this;
                }

                public Builder End()
                {
                    return _builder;
                }
            }
            
        }
    }
}