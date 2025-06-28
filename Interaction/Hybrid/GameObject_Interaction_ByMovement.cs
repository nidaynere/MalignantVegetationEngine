using EsnetEngine.Pooling;
using Unity.Entities;
using UnityEngine;

namespace MalignantVegetationEngine
{
    [AddComponentMenu ("MalignantVegetationEngine/" + nameof (GameObject_Interaction_ByMovement))]
    [DisallowMultipleComponent]
    internal sealed class GameObject_Interaction_ByMovement : MonoBehaviour, IPoolEnabled, IPoolDisabled
    {
        [Header("Interaction settings")]
        [SerializeField] private float speed = 1;
        [SerializeField] private float radius = 2;
        [SerializeField, Min(0)] private float power = 1;

        [Header ("Continous")]
        [SerializeField] private bool loop = false;
        [SerializeField] private float renewAfterMovement = 0.2f;

        private Vector3 lastPosition;
        private EntityQuery entityQuery;
        private float movementSq;

        private bool isPoolEnabled;

        private void Awake()
        {
            entityQuery = World.
                DefaultGameObjectInjectionWorld.
                EntityManager.
                CreateEntityQuery(typeof(InteractionPoint));

            movementSq = renewAfterMovement * renewAfterMovement;
        }

        public void OnPostEnabled()
        {
            lastPosition = transform.position;
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

            if (!loop)
            {
                return;
            }

            var currentPosition = transform.position;
            var distSq = Vector3.SqrMagnitude (lastPosition - currentPosition);
            if (distSq > movementSq)
            {
                lastPosition = currentPosition;
                CreateInteraction();
            }
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