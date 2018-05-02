namespace Unscientific.ECS.Modules.BehaviourTree
{
    public static class WorldBuilderExtensions
    {
        public static BehaviourTreeModule.Builder<TScope> UsingBehaviourTreeModule<TScope>(this World.Builder self) where TScope : IScope
        {
            return new BehaviourTreeModule.Builder<TScope>(self);
        }
        
        public static World.Builder UsingBehaviourTreeModuleWithDefaults<TScope>(this World.Builder self) where TScope : IScope
        {
            return self.UsingBehaviourTreeModule<TScope>().End();
        }

    }
}