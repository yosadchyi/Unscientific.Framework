namespace Unscientific.ECS.Features.Steering2D
{
    public class SteeringBehaviourBuilderFinalizer
    {
        private readonly SteeringBehaviour _behaviour;

        public SteeringBehaviourBuilderFinalizer(SteeringBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public SteeringBehaviour Build()
        {
            return _behaviour;
        }
    }
}