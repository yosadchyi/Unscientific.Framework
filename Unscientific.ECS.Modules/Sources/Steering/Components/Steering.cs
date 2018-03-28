namespace Unscientific.ECS.Modules.Steering
{
    public struct Steering
    {
        public readonly SteeringBehaviour SteeringBehaviour;

        public Steering(SteeringBehaviour steeringBehaviour)
        {
            SteeringBehaviour = steeringBehaviour;
        }
    }
}