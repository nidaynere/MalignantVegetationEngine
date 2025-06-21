using Unity.Burst;
using Unity.Entities;

namespace MalignantVegetationEngine
{
    [UpdateAfter(typeof(InteractionBufferWriterSystem))]
    [BurstCompile]
    public partial struct InteractionDisposeSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {

        }

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new InteractionDisposeJob().Schedule(state.Dependency);
        }

        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        private partial struct InteractionDisposeJob : IJobEntity
        {
            [BurstCompile]
            public void Execute(DynamicBuffer<InteractionPoint> interactionPoints)
            {
                for (int i = interactionPoints.Length - 1; i >= 0; i--) 
                { 
                    if (interactionPoints[i].runtime_Progress01 >= 1)
                    {
                        interactionPoints.RemoveAt(i);
                    }
                }
            }
        }
    }
}
