using EsnetEngine.Pooling;
using Unity.Entities;
using UnityEngine;

namespace MalignantVegetationEngine
{
    [AddComponentMenu ("MalignantVegetationEngine/" + nameof (VegetationInteractionGameObject))]
    [DisallowMultipleComponent]
    internal sealed class VegetationInteractionGameObject : MonoBehaviour, IPoolEnabled, IPoolDisabled
    {
        [Header("Interaction settings")]
        [SerializeField] private float speed = 1;
        [SerializeField] private float radius = 2;

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
            CreateInteraction();
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

            buffer.Add(new InteractionPoint()
            {
                interactionPosition = transform.position,
                maxRadius = radius,
                speed = speed
            });
        }
    }
}