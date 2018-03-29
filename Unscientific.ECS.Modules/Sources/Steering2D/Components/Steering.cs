namespace Unscientific.ECS.Modules.Steering2D
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