using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MalignantVegetationEngine
{
    [UpdateAfter (typeof (InteractionAnimationSystem))]

    public partial struct InteractionBufferWriterSystem : ISystem
    {
        private ComputeBuffer buffer;

        public void OnCreate(ref SystemState state)
        {
            buffer = new ComputeBuffer(InteractionGlobalSettings.k_maxInteractors, sizeof(float) * 3);
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new InteractionBufferWriterJob() 
            { 
                buffer = buffer 
            }.
            Schedule(state.Dependency);
        }

        public void OnDestroy(ref SystemState state)
        {
            buffer.Dispose();
        }

        private struct BufferData
        {
            public float3 Position;
            public float Radius;
            public float IsValid;

            public int Size()
            {
                return 
                    sizeof(float) * 3 + 
                    sizeof(float) +
                    sizeof(float);
            }
        }

        private partial struct InteractionBufferWriterJob : IJobEntity
        {
            public ComputeBuffer buffer;

            public void Execute(DynamicBuffer<InteractionPoint> interactionPoints)
            {
                var data = new NativeArray<BufferData>(InteractionGlobalSettings.k_maxInteractors, Allocator.Temp);
                var totalInteractions = interactionPoints.Length;

                for (int i=0; i<InteractionGlobalSettings.k_maxInteractors; i++)
                {
                    if (i >= totalInteractions)
                    {
                        return;
                    }

                    var element = interactionPoints[i];
                    var radius = math.sin(math.sqrt (element.runtime_Progress01) * 3.14f);

                    data[i] = new BufferData()
                    {
                        IsValid = 1,
                        Radius = radius,
                        Position = element.interactionPosition,
                    };
                }
              
                buffer.SetData(data);

                Shader.SetGlobalBuffer("_InteractionBuffer", buffer);

                data.Dispose();
            }
        }
    }
}
