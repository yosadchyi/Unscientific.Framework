using System;

namespace Unscientific.ECS
{
    public interface IContext
    {
        void Clear();
        Type ScopeType { get;  }
    }
}