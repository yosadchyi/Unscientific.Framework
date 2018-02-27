namespace Unscientificlab.ECS
{
    public class TryingToDestroyReferencedEntity<TScope> : global::System.Exception
    {
        public TryingToDestroyReferencedEntity(int id) :
            base (string.Format("Destoroying referenced entity {0}#{1}!", typeof(TScope).Name, id))
        {
        }
    }
}