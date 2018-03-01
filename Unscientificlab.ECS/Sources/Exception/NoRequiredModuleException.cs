using System;

namespace Unscientificlab.ECS
{
    public class NoRequiredModuleException : Exception
    {
        public NoRequiredModuleException(Type moduleType): base(string.Format("Required module `{0}` is not in import list!", moduleType.Name))
        {
        }
    }
}