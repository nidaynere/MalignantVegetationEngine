using EsnetEngine.Pooling;
using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace MalignantVegetationEngine
{
    [AddComponentMenu("MalignantVegetationEngine/" + nameof(GameObject_Interaction_Burst))]
    [DisallowMultipleComponent]
    internal sealed class GameObject_Interaction_Burst : MonoBehaviour, IPoolEnabled, IPoolDisabled
    {
        [Header("Interaction settings")]
        [SerializeField] private Vector2 speedRange = new Vector2(0.5f, 1);
        [SerializeField] private Vector2 radiusRange = new Vector2 (2f, 3f);
        [SerializeField] private Vector2 powerRange = new Vector2(0.5f, 1f);

        [Header("Burst")]
        [SerializeField] private ushort burstCount = 4;
        [SerializeField] private Vector2 burstSpeed = new Vector2(0.1f, 0.3f);

        private EntityQuery entityQuery;

        private void Awake()
        {
            entityQuery = World.
                DefaultGameObjectInjectionWorld.
                EntityManager.
                CreateEntityQuery(typeof(InteractionPoint));
        }

        public void OnPostEnabled()
        {
            StartCoroutine(CreateInteraction());
        }

        public void OnPreDisabled()
        {

        }

        public void OnPreEnabled()
        {

        }

        private IEnumerator CreateInteraction()
        {
            for (int i=0; i<burstCount; i++)
            {
                if (!entityQuery.TryGetSingletonBuffer<InteractionPoint>(out var buffer))
                {
                    yield break;
                }

                entityQuery.CompleteDependency();

                buffer.Add(new InteractionPoint()
                {
                    interactionPower = Random.Range(powerRange.x, powerRange.y),
                    interactionPosition = transform.position,
                    maxRadius = Random.Range(radiusRange.x, radiusRange.y),
                    speed = Random.Range(speedRange.x, speedRange.y),
                });

                yield return new WaitForSeconds(Random.Range(burstSpeed.x, burstSpeed.y));
            }
        }
    }
}