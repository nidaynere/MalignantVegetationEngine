using Unity.Burst;
using Unity.Entities;

namespace MalignantVegetationEngine
{
    [BurstCompile]
    public partial struct InteractionAnimationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new InteractionAnimationJob() 
            { 
                delta = SystemAPI.Time.DeltaTime 
            }.
            Schedule(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        private partial struct InteractionAnimationJob : IJobEntity
        {
            public float delta;

            [BurstCompile]
            public void Execute(DynamicBuffer<InteractionPoint> interactionPoints)
            {
                for (int i = interactionPoints.Length - 1; i>=0; i--)
                {
                    var element = interactionPoints[i];
                    element.runtime_Progress01 += delta * element.speed;
                }
            }
        }
    }
}
