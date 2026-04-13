using UnityEngine;
using UnityEditor;

public class SetPointFiltering
{
    [MenuItem("Tools/Set All Textures to Point Filtering")]
    static void SetAllTexturesToPoint()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D");
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null && importer.filterMode != FilterMode.Point)
            {
                importer.filterMode = FilterMode.Point;
                AssetDatabase.ImportAsset(path);
                count++;
            }
        }

        Debug.Log($"Set {count} textures to Point filtering.");
        AssetDatabase.SaveAssets();
    }
}