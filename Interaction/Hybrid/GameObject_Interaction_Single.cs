using EsnetEngine.Pooling;
using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace MalignantVegetationEngine
{
    [AddComponentMenu("MalignantVegetationEngine/" + nameof(GameObject_Interaction_Single))]
    [DisallowMultipleComponent]
    internal sealed class GameObject_Interaction_Single : MonoBehaviour, IPoolEnabled
    {
        [Header("Interaction settings")]
        [SerializeField] private float speed = 1;
        [SerializeField] private float radius = 2;
        [SerializeField, Min(0)] private float power = 1;

        [Header ("Delay")]
        [SerializeField] private float delay = 0;

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

        public void OnPreEnabled()
        {

        }

        private IEnumerator CreateInteraction()
        {
            yield return new WaitForSeconds(delay);

            if (!entityQuery.TryGetSingletonBuffer<InteractionPoint>(out var buffer))
            {
                yield break;
            }

            entityQuery.CompleteDependency();

            buffer.Add(new InteractionPoint()
            {
                interactionPosition = transform.position,
                interactionPower = power,
                maxRadius = radius,
                speed = speed
            });
        }
    }
}