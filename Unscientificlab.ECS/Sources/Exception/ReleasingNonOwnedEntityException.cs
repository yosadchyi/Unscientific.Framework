namespace Unscientificlab.ECS.Exception
{
    public class ReleasingNonOwnedEntityException : global::System.Exception
    {
        public ReleasingNonOwnedEntityException(object owner, int id) : base(string.Format("Releasing entity #{0} not owned by {1}!", id, owner))
        {
        }
    }
}