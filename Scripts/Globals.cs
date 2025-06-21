using UnityEngine;

namespace MalignantVegetationEngine
{
    [AddComponentMenu (menuName: "MalignantVegetationEngine/Globals")]
    [ExecuteAlways]
    [DisallowMultipleComponent]
    internal sealed class Globals : MonoBehaviour
    {
        private int globalWindNormalStartInt;
        private int globalWindNormalEndInt;
        private int globalWindPowerInt;
        private int globalWindStutterInt;

        private int globalNoiseScaleInt;

        private int globalColorFilterColorInt;
        private int globalColorFilterIntensityInt;

        [Header("Noise Settings")]
        [SerializeField] private Vector2 noiseScale = new Vector2(0.003f, 0.003f);

        [Header ("Wind Settings")]
        [SerializeField] private Vector3 windNormalStart = new Vector3 (0,0, -1);
        [SerializeField] private Vector3 windNormalEnd = new Vector3(0, 0, -2);
        [SerializeField, Min(0)] private float windPower = 2;
        [SerializeField, Min(0)] private float windFlutter = 0.05f;

        [Header("Color Filter")]
        [SerializeField, Range (0f, 1f)] private float colorFilterIntensity = 0f;
        [SerializeField, ColorUsage(false, true)] private Color colorFilter = Color.white;

        void OnEnable()
        {
            globalWindNormalStartInt = Shader.PropertyToID("_globalWindNormalStart");
            globalWindNormalEndInt = Shader.PropertyToID("_globalWindNormalEnd");
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
            Shader.SetGlobalVector (globalWindNormalStartInt, windNormalStart);
            Shader.SetGlobalVector (globalWindNormalEndInt, windNormalEnd);
            Shader.SetGlobalFloat  (globalWindPowerInt, windPower);

            Shader.SetGlobalColor  (globalColorFilterColorInt, colorFilter);
            Shader.SetGlobalFloat  (globalColorFilterIntensityInt, colorFilterIntensity);
        }
    }
}

