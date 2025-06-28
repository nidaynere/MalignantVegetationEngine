using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MalignantVegetationEngine
{
    [BurstCompile]
    internal struct BufferData
    {
        public float3 Position;
        public float Radius;
        public float Power;

        [BurstCompile]
        public static int Size()
        {
            return
                sizeof(float) * 3 +
                sizeof(float) +
                sizeof(float);
        }
    }

    internal partial class InteractionBufferWriterSystem : SystemBase
    {
        private ComputeBuffer buffer;

        protected override void OnCreate()
        {
            CheckedStateRef.RequireForUpdate<InteractionPoint>();
            buffer = new ComputeBuffer(InteractionGlobalSettings.k_maxInteractors, BufferData.Size());
        }

        protected override void OnDestroy()
        {
            buffer.Dispose();
        }

        protected override void OnUpdate()
        {
            Entities
                .ForEach(
                    (DynamicBuffer<InteractionPoint> interactionPoints) =>
                    {
                        var data = new NativeArray<BufferData>(InteractionGlobalSettings.k_maxInteractors, Allocator.Temp);
                        var totalInteractions = interactionPoints.Length;

                        for (int i = 0; i < InteractionGlobalSettings.k_maxInteractors; i++)
                        {
                            if (i >= totalInteractions)
                            {
                                break;
                            }

                            var element = interactionPoints[i];
                            var radius = element.maxRadius * math.sin(math.sqrt(element.runtime_Progress01) * 3.14f);

                            data[i] = new BufferData()
                            {
                                Radius = radius,
                                Position = element.interactionPosition,
                                Power = element.interactionPower
                            };
                        }

                        buffer.SetData(data);

                        Shader.SetGlobalBuffer("_InteractionBuffer", buffer);

                        data.Dispose();
                    }
                ).WithoutBurst().Run();

        }
    }
}
