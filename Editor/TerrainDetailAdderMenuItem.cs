using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MalignantVegetationEngine
{
    internal sealed class TerrainDetailAdderMenuItem
    {
        [MenuItem("Assets/MalignantVegetationEngine/IncludeDetailsToTerrain (clear first)", false, 20)]
        private static void IncludeDetailsToTerrain_ClearFirst()
        {
            var activeTerrain = Terrain.activeTerrain;
            var terrainData = activeTerrain.terrainData;
            terrainData.detailPrototypes = new DetailPrototype[0];

            IncludeDetailsToTerrain();
        }

        [MenuItem("Assets/MalignantVegetationEngine/IncludeDetailsToTerrain", false, 20)]
        private static void IncludeDetailsToTerrain()
        {
            var activeTerrain = Terrain.activeTerrain;
            var terrainData = activeTerrain.terrainData;
            var currentPrototypes = terrainData.detailPrototypes;

            var include = Selection.
                gameObjects.
                Select(x => new DetailPrototype() 
                { 
                     alignToGround = 1,
                     density = 3,
                     minHeight = 1,
                     maxHeight = 2,
                     minWidth = 1,
                     maxWidth = 2,
                     renderMode = DetailRenderMode.VertexLit,
                     useInstancing = true,
                     usePrototypeMesh = true,
                     useDensityScaling = true,
                     prototype = x
                });

            currentPrototypes = currentPrototypes.Concat(include).ToArray();
            terrainData.detailPrototypes = currentPrototypes;
        }
    }
}
