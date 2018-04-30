namespace Unscientific.ECS.Modules.Steering2D
{
    public class WeightedBehaviourBuilderFinalizer<TResult>
    {
        private readonly TResult _result;

        public WeightedBehaviourBuilderFinalizer(TResult result)
        {
            _result = result;
        }

        public TResult End()
        {
            return _result;
        }
    }
}