using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace MalignantVegetationEngine
{
    [BurstCompile]
    internal partial struct InteractionPoint : IBufferElementData
    {
        public float3 interactionPosition;
        public float maxRadius;
        public float speed;
        public float runtime_Progress01;
    }
}
