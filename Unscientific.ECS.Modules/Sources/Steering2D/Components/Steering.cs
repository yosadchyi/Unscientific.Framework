namespace Unscientific.ECS.Features.Steering2D
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