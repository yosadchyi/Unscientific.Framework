namespace Unscientificlab.ECS.Exception
{
    public class ReleasingRetainedEntityException<TScope> : global::System.Exception
    {
        public ReleasingRetainedEntityException(int id) :
            base (string.Format("Releasing free entity {0}#{1}!", typeof(TScope).Name, id))
        {
        }
    }
}