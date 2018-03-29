namespace Unscientific.ECS.Modules.Steering2D
{
    public abstract class GroupSteeringBehaviour: SteeringBehaviour
    {
        public readonly Proximity Proximity;

        protected GroupSteeringBehaviour (Proximity proximity)
        {
            Proximity = proximity;
        }
    }
}
