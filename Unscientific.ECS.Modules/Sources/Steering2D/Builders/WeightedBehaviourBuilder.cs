using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class WeightedBehaviourBuilder<TFinalizeResult> :
        SteeringBehaviourBuilderBase<WeightedBehaviourBuilderFinalizer<TFinalizeResult>, TFinalizeResult>
    {
        private readonly AcceptSteeringBehaviour _accept;
        private readonly TFinalizeResult _result;
        private readonly Fix _weight;

        public WeightedBehaviourBuilder(AcceptSteeringBehaviour accept, TFinalizeResult result, Fix weight)
        {
            _accept = accept;
            _result = result;
            _weight = weight;
        }

        protected override WeightedBehaviourBuilderFinalizer<TFinalizeResult> GetBuilderMethodResult()
        {
            return new WeightedBehaviourBuilderFinalizer<TFinalizeResult>(_result);
        }

        protected override TFinalizeResult GetFinalizeResult()
        {
            return _result;
        }

        protected override void Accept(SteeringBehaviour behaviour)
        {
            _accept(new WeightedBehaviour(_weight, behaviour));
        }
    }
}