using UnityEditor;
using UnityEngine;

public class LODRemover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    [MenuItem ("LODRemover/Remove")]
    // Update is called once per frame
    static void Run()
    {
        var objects = Selection.gameObjects;
        foreach (var selection in objects)
        {
            // open on prefab mode.
            var path = AssetDatabase.GetAssetPath(selection);
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);
            if (!prefabRoot.TryGetComponent<LODGroup>(out var lodGroup))
            {
                // Unload the Prefab contents
                PrefabUtility.UnloadPrefabContents(prefabRoot);
                continue;
            }

            var lods = lodGroup.GetLODs();
            for (int i=1; i<lodGroup.lodCount; i++)
            {
                var lod = lods[i];
                var renderers = lod.renderers;
                foreach (var renderer in renderers)
                {
                    DestroyImmediate(renderer.gameObject, true);
                }
            }

            DestroyImmediate(lodGroup, true);

            var rnd = lods[0].renderers[0];
            UnityEditorInternal.ComponentUtility.CopyComponent(rnd);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(prefabRoot);

            var filter = rnd.GetComponent<MeshFilter>();
            UnityEditorInternal.ComponentUtility.CopyComponent(filter);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(prefabRoot);

            DestroyImmediate(rnd, true);
            DestroyImmediate(filter, true);

            // Save the changes back to the Prefab asset
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
    }
}
