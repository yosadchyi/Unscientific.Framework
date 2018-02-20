namespace Unscientificlab.ECS.Exception
{
    public class ReleasingNonRetainedEntityException : global::System.Exception
    {
        public ReleasingNonRetainedEntityException(int id) :
            base (string.Format("Releasing free entity #{0}!", id))
        {
        }
    }
}