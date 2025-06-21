using Unity.Burst;

namespace MalignantVegetationEngine
{
    [BurstCompile]
    public struct InteractionGlobalSettings
    {
        public const int k_maxInteractors = 512;
    }
}