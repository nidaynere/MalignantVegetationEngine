using UnityEngine;

namespace MalignantVegetationEngine
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    internal sealed class GlobalWind : MonoBehaviour
    {
        private int globalWindPowerInt;
        private int globalWindSpeedInt;

        [SerializeField] private Vector3 windPower;
        [SerializeField, Min (0)] private float windSpeed;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            globalWindPowerInt = Shader.PropertyToID("_GlobalWindPower");
            globalWindSpeedInt = Shader.PropertyToID("_GlobalWindSpeed");
        }

        // Update is called once per frame
        void Update()
        {
            Shader.SetGlobalVector(globalWindPowerInt, windPower);
            Shader.SetGlobalFloat(globalWindSpeedInt, windSpeed);
        }
    }
}

