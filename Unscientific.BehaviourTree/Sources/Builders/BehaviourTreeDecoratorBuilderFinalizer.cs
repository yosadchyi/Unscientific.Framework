namespace Unscientific.BehaviourTree
{
    public interface IBehaviourTreeDecoratorBuilderFinalizer<out TFinalizeResult>
    {
        TFinalizeResult End();
    }
}