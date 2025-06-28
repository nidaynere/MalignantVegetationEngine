using EsnetEngine.Pooling;
using Unity.Entities;
using UnityEngine;

namespace MalignantVegetationEngine
{
    [AddComponentMenu("MalignantVegetationEngine/" + nameof(GameObject_Interaction_Loop))]
    [DisallowMultipleComponent]
    internal sealed class GameObject_Interaction_Loop : MonoBehaviour, IPoolEnabled, IPoolDisabled
    {
        [Header("Interaction settings")]
        [SerializeField] private float speed = 1;
        [SerializeField] private float radius = 2;
        [SerializeField, Min(0)] private float power = 1;

        [Header("Loop")]
        [SerializeField] private float frequency = 0.5f;

        private EntityQuery entityQuery;
        private bool isPoolEnabled;
        private float nextInteraction;

        private void Awake()
        {
            entityQuery = World.
                DefaultGameObjectInjectionWorld.
                EntityManager.
                CreateEntityQuery(typeof(InteractionPoint));
        }

        public void OnPostEnabled()
        {
            isPoolEnabled = true;
        }

        public void OnPreDisabled()
        {
            isPoolEnabled = false;
        }

        public void OnPreEnabled()
        {

        }

        private void FixedUpdate()
        {
            if (!isPoolEnabled)
            {
                return;
            }

            var time = Time.time;

            if (nextInteraction > time)
            {
                return;
            }

            nextInteraction = time + frequency;

            CreateInteraction();
        }

        private void CreateInteraction()
        {
            if (!entityQuery.TryGetSingletonBuffer<InteractionPoint>(out var buffer))
            {
                return;
            }

            entityQuery.CompleteDependency();

            buffer.Add(new InteractionPoint()
            {
                interactionPower = power,
                interactionPosition = transform.position,
                maxRadius = radius,
                speed = speed
            });
        }
    }
}