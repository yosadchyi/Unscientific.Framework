namespace Unscientificlab.ECS
{
    public class ReleasingNonRetainedEntityException<TScope> : System.Exception
    {
        public ReleasingNonRetainedEntityException(int id) :
            base (string.Format("Releasing free entity {0}#{1}!", typeof(TScope).Name, id))
        {
        }
    }
}