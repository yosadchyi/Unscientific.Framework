namespace Unscientific.ECS.Features.Steering2D
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