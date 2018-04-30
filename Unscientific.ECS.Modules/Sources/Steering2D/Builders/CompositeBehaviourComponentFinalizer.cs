using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class CompositeBehaviourComponentFinalizer<TFinalizerResult>
    {
        private readonly AcceptSteeringBehaviour _accept;
        private readonly SteeringBehaviour _behaviour;
        private readonly TFinalizerResult _result;

        public CompositeBehaviourComponentFinalizer(AcceptSteeringBehaviour accept, SteeringBehaviour behaviour, TFinalizerResult result)
        {
            _accept = accept;
            _behaviour = behaviour;
            _result = result;
        }

        public TFinalizerResult WithWeight(Fix weight)
        {
            _accept(weight == Fix.One ? _behaviour : new WeightedBehaviour(weight, _behaviour));
            return _result;
        }
    }
}