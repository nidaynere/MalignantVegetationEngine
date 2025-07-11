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
            Shader.EnableKeyword("_CONDITION1");
            World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity(typeof(InteractionPoint));
        }

        private void OnDestroy()
        {
            Shader.DisableKeyword("_CONDITION1");
        }
    }
}
