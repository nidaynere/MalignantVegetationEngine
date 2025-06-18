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

        private int globalColorFilterColorInt;
        private int globalColorFilterIntensityInt;

        [Header("Noise Settings")]
        [SerializeField] private Vector2 noiseScale;
        [SerializeField] private float noisePower;

        [Header ("Wind Settings")]
        [SerializeField] private Vector3 windNormal;
        [SerializeField, Min (0)] private float windPower;

        [Header("Color Filter")]
        [SerializeField, Range (0f, 1f)] private float colorFilterIntensity;
        [SerializeField, ColorUsage(false, true)] private Color colorFilter;

        void OnEnable()
        {
            globalWindNormalInt = Shader.PropertyToID("_globalWindDirectionNormal");
            globalWindPowerInt = Shader.PropertyToID("_globalWindDirectionPower");

            globalNoiseScaleInt = Shader.PropertyToID("_globalWindNoiseTiling");
            globalNoisePowerInt = Shader.PropertyToID("_globalWindNoisePower");

            globalColorFilterColorInt = Shader.PropertyToID("_globalColorFilterColor");
            globalColorFilterIntensityInt = Shader.PropertyToID("_globalColorFilterIntensity");
        }

        void Update()
        {
            Shader.SetGlobalVector (globalNoiseScaleInt, noiseScale);
            Shader.SetGlobalFloat  (globalNoisePowerInt, noisePower);

            Shader.SetGlobalVector (globalWindNormalInt, windNormal.normalized);
            Shader.SetGlobalFloat  (globalWindPowerInt, windPower);

            Shader.SetGlobalColor(globalColorFilterColorInt, colorFilter);
            Shader.SetGlobalFloat(globalColorFilterIntensityInt, colorFilterIntensity);
        }
    }
}

