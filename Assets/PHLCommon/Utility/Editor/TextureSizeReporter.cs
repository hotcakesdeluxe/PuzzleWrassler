using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TextureSizeReporter
{

    public static List<T> FindAssetByType<T>(string assetTypeName) where T : Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", assetTypeName));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }

    public static List<string> FindPathsByType(string assetTypeName)
    {
        List<string> paths = new List<string>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", assetTypeName));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            paths.Add(assetPath);
        }
        return paths;
    }

    [MenuItem("Tools/PHL/Report Texture Sizes")]
    private static void ReportTextureSizes()
    {
        List<string> paths = FindPathsByType("Texture2D");

        foreach(string path in paths)
        {
            TextureImporter tImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            if (tImporter != null)
            {
                if (tImporter.maxTextureSize > 1025)
                {
                    Texture asset = AssetDatabase.LoadAssetAtPath<Texture>(path);
                    if (asset != null)
                    {
                        if (asset.width > 1025 || asset.height > 1025)
                        {
                            Debug.Log(path);
                        }
                    }
                }
            }
        }
    }
}
