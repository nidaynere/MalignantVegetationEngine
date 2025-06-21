using Unity.Entities;
using UnityEngine;

namespace MalignantVegetationEngine
{
    [AddComponentMenu("MalignantVegetationEngine/" + nameof(InteractionInitializer))]
    [DisallowMultipleComponent]

    public class InteractionInitializer : MonoBehaviour
    {
        void Start()
        {
            World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity(typeof(InteractionPoint));
        }
    }
}
