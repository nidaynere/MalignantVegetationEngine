
using UnityEngine;

namespace MalignantVegetationEngine
{
    [AddComponentMenu ("MalignantVegetationEngine/" + nameof (VegetationInteractionGameObject))]
    [DisallowMultipleComponent]
    internal sealed class VegetationInteractionGameObject : MonoBehaviour
    {
        [Header("Interaction settings")]
        [SerializeField] private bool loop = false;
        [SerializeField] private float speed = 1;
        [SerializeField] private float renewAfterMovement = 0.2f;
    }
}