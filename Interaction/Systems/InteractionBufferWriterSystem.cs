using NUnit.Framework;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MalignantVegetationEngine
{
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
            Assert.IsNotNull(buffer);

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
                            var radius = math.sin(math.sqrt(element.runtime_Progress01) * 3.14f);

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
                ).WithoutBurst().Run();

        }

        private struct BufferData
        {
            public float3 Position;
            public float Radius;
            public float IsValid;

            public static int Size()
            {
                return
                    sizeof(float) * 3 +
                    sizeof(float) +
                    sizeof(float);
            }
        }
    }
}
