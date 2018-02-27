namespace Unscientificlab.ECS.Exception
{
    public class ReleasingNonRetainedEntityException<TScope> : global::System.Exception
    {
        public ReleasingNonRetainedEntityException(int id) :
            base (string.Format("Releasing free entity {0}#{1}!", typeof(TScope).Name, id))
        {
        }
    }
}