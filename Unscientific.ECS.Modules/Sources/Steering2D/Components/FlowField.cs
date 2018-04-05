using Unscientific.ECS.Modules.Navigation2D;

namespace Unscientific.ECS.Modules.Steering2D
{
    public struct FlowField
    {
        public readonly IFlowField Field;

        public FlowField(IFlowField field)
        {
            Field = field;
        }
    }
}