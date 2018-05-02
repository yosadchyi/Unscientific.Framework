using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class SteeringBehaviourBuilder :
        SteeringBehaviourBuilderBase<SteeringBehaviourBuilderFinalizer, SteeringBehaviourBuilderFinalizer>
    {
        private SteeringBehaviour _behaviour;

        protected override SteeringBehaviourBuilderFinalizer GetBuilderMethodResult()
        {
            return GetFinalizeResult();
        }

        protected override SteeringBehaviourBuilderFinalizer GetFinalizeResult()
        {
            return new SteeringBehaviourBuilderFinalizer(_behaviour);
        }

        protected override void Accept(SteeringBehaviour behaviour)
        {
            _behaviour = behaviour;
        }
    }
}