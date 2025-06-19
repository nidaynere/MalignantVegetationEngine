using UnityEngine;

namespace MalignantVegetationEngine
{
    [AddComponentMenu (menuName: "MalignantVegetationEngine/Globals")]
    [ExecuteAlways]
    [DisallowMultipleComponent]
    internal sealed class Globals : MonoBehaviour
    {
        private int globalWindNormalInt;
        private int globalWindPowerInt;
        private int globalWindStutterInt;

        private int globalNoiseScaleInt;

        private int globalColorFilterColorInt;
        private int globalColorFilterIntensityInt;

        [Header("Noise Settings")]
        [SerializeField] private Vector2 noiseScale;
        [SerializeField] private float vertexBasedNoisePower;

        [Header ("Wind Settings")]
        [SerializeField] private Vector3 windNormal;
        [SerializeField, Min (0)] private float windPower;
        [SerializeField, Min(0)] private float windFlutter;

        [Header("Color Filter")]
        [SerializeField, Range (0f, 1f)] private float colorFilterIntensity;
        [SerializeField, ColorUsage(false, true)] private Color colorFilter;

        void OnEnable()
        {
            globalWindNormalInt = Shader.PropertyToID("_globalWindDirectionNormal");
            globalWindPowerInt = Shader.PropertyToID("_globalWindDirectionPower");
            globalWindStutterInt = Shader.PropertyToID("_globalWindDirectionStutter");

            globalNoiseScaleInt = Shader.PropertyToID("_globalWindNoiseTiling");

            globalColorFilterColorInt = Shader.PropertyToID("_globalColorFilterColor");
            globalColorFilterIntensityInt = Shader.PropertyToID("_globalColorFilterIntensity");
        }

        void Update()
        {
            Shader.SetGlobalVector (globalNoiseScaleInt, noiseScale);

            Shader.SetGlobalFloat  (globalWindStutterInt, windFlutter);
            Shader.SetGlobalVector (globalWindNormalInt, windNormal.normalized);
            Shader.SetGlobalFloat  (globalWindPowerInt, windPower);

            Shader.SetGlobalColor  (globalColorFilterColorInt, colorFilter);
            Shader.SetGlobalFloat  (globalColorFilterIntensityInt, colorFilterIntensity);
        }
    }
}

