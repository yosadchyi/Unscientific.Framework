using Unscientific.ECS.Modules.Navigation;

namespace Unscientific.ECS.Modules.Steering2D
{
    public struct FlowField
    {
        public IFlowField Field;

        public FlowField(IFlowField field)
        {
            Field = field;
        }
    }
}