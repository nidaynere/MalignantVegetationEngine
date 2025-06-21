using UnityEditor;
using UnityEngine;

public class LODRemover : MonoBehaviour
{
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
            DestroyImmediate(lodGroup, true);

            var selected = lods[1];
            var rnd = selected.renderers[0];
            rnd.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            UnityEditorInternal.ComponentUtility.CopyComponent(rnd);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(prefabRoot);

            var filter = rnd.GetComponent<MeshFilter>();
            UnityEditorInternal.ComponentUtility.CopyComponent(filter);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(prefabRoot);

            for (int i= prefabRoot.transform.childCount-1; i>=0; i--)
            {
                DestroyImmediate(prefabRoot.transform.GetChild(i).gameObject, true);
            }

            // Save the changes back to the Prefab asset
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
    }
}
