using UnityEngine;

namespace MalignantVegetationEngine
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    internal sealed class GlobalWind : MonoBehaviour
    {
        private int globalWindNormalInt;
        private int globalWindPowerInt;
        private int globalNoiseScaleInt;
        private int globalNoisePowerInt;

        [Header("Noise Settings")]
        [SerializeField] private Vector2 noiseScale;
        [SerializeField] private float noisePower;

        [Header ("Wind Settings")]
        [SerializeField] private Vector3 windNormal;
        [SerializeField, Min (0)] private float windPower;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void OnEnable()
        {
            globalWindNormalInt = Shader.PropertyToID("_globalWindDirectionNormal");
            globalWindPowerInt = Shader.PropertyToID("_globalWindDirectionPower");
            globalNoiseScaleInt = Shader.PropertyToID("_globalWindNoiseTiling");
            globalNoisePowerInt = Shader.PropertyToID("_globalWindNoisePower");
        }

        // Update is called once per frame
        void Update()
        {
            Shader.SetGlobalVector (globalNoiseScaleInt, noiseScale);
            Shader.SetGlobalFloat  (globalNoisePowerInt, noisePower);

            Shader.SetGlobalVector (globalWindNormalInt, windNormal.normalized);
            Shader.SetGlobalFloat  (globalWindPowerInt, windPower);
        }
    }
}

