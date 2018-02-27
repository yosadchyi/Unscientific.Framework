namespace Unscientificlab.ECS.Exception
{
    public class ReleasingNonOwnedEntityException<TScope> : global::System.Exception
    {
        public ReleasingNonOwnedEntityException(object owner, int id) : base(string.Format("Releasing entity {0}#{1} not owned by {2}!", typeof(TScope).Name, id, owner))
        {
        }
    }
}