using Unscientific.ECS.Features.Navigation2D;

namespace Unscientific.ECS.Features.Steering2D
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