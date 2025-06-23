using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MalignantVegetationEngine
{
    internal sealed class TerrainTreeAdderMenuItem
    {
        [MenuItem("Assets/MalignantVegetationEngine/IncludeTreeToTerrain (clear first)", false, 20)]
        private static void IncludeTreesToTerrain_ClearFirst()
        {
            var activeTerrain = Terrain.activeTerrain;
            var terrainData = activeTerrain.terrainData;
            terrainData.treePrototypes = new TreePrototype[0];

            IncludeTreesToTerrain();
        }

        [MenuItem("Assets/MalignantVegetationEngine/IncludeTreesToTerrain", false, 20)]
        private static void IncludeTreesToTerrain()
        {
            var activeTerrain = Terrain.activeTerrain;
            var terrainData = activeTerrain.terrainData;
            var currentPrototypes = terrainData.treePrototypes;

            var include = Selection.
                gameObjects.
                Select(x => new TreePrototype() { prefab = x });

            currentPrototypes = currentPrototypes.Concat(include).ToArray();
            terrainData.treePrototypes = currentPrototypes;
        }
    }
}
