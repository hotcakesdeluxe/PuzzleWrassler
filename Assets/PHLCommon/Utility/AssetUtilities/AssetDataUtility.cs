using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
public class AssetDataUtility
{

    public T FindAsset<T>(string type, string filter) where T : UnityEngine.Object
    {
        return FindAssets<T>(type).FirstOrDefault( a => a.name == filter);
    }

    public List<T> FindAssets<T>(string filter) where T : UnityEngine.Object
    {        
        List<string> pieces = AssetDatabase.FindAssets("t:"+filter).ToList();
        List<T> assets = new List<T>();
        
        pieces.ForEach(p =>
        {
            string path = AssetDatabase.GUIDToAssetPath(p);
            T proto = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
            assets.Add(proto);
        });	

        return assets;
    }
}
#endif

