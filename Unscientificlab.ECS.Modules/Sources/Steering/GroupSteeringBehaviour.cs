namespace Unscientificlab.ECS.Modules.Steering
{
    public abstract class GroupSteeringBehaviour: SteeringBehaviour
    {
        public Proximity Proximity;

        protected GroupSteeringBehaviour (Proximity proximity)
        {
            Proximity = proximity;
        }
    }
}
