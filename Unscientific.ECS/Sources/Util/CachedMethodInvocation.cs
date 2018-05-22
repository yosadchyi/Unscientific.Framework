using System.Reflection;

namespace Unscientific.ECS
{
    public class CachedMethodInvocation
    {
        private readonly MethodInfo _methodInfo;
        private readonly object _instance;
        private readonly object[] _parameters;

        public CachedMethodInvocation(MethodInfo methodInfo, object instance, object[] parameters)
        {
            _methodInfo = methodInfo;
            _instance = instance;
            _parameters = parameters;
        }

        public void Invoke()
        {
            _methodInfo.Invoke(_instance, _parameters);
        }
    }
}