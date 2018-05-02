namespace Unscientific.ECS.Modules.BehaviourTree
{
    public static class WorldBuilderExtensions
    {
        public static BehaviourTreeModule.Builder<TScope> UsesBehaviourTreeModule<TScope>(this World.Builder self) where TScope : IScope
        {
            return new BehaviourTreeModule.Builder<TScope>(self);
        }
        
        public static World.Builder UsesBehaviourTreeModuleWithDefaults<TScope>(this World.Builder self) where TScope : IScope
        {
            return self.UsesBehaviourTreeModule<TScope>().End();
        }

    }
}