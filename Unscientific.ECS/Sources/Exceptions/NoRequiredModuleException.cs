using System;

namespace Unscientific.ECS
{
    public class NoRequiredModuleException : Exception
    {
        public NoRequiredModuleException(Type moduleType): base($"Required module `{moduleType.Name}` is not in import list!")
        {
        }
    }
}