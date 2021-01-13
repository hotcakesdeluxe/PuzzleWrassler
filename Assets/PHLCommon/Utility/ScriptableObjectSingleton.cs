using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class ScriptableObjectSingleton<T> : ScriptableObject
    where T : ScriptableObject
    {
        private static T _instance = null;

        public static T instance
        {
            get
            {
                if (!_instance)
                {
                    T[] objs = null;

#if UNITY_EDITOR
                    string[] objsGUID = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
                    int count = objsGUID.Length;
                    objs = new T[count];
                    for (int i = 0; i < count; i++)
                    {
                        objs[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(objsGUID[i]));
                    }
#else
                objs = Resources.FindObjectsOfTypeAll<T>();
#endif
                    if (objs.Length == 0)
                    {
                        Debug.LogError("No asset of type \"" + typeof(T).Name + "\" has been found in loaded resources. Please create a new one and add it to the \"Preloaded Assets\" array in Edit > Project Settings > Player > Other Settings.");
                    }
                    else if (objs.Length > 1)
                    {
                        Debug.LogError("There's more than one asset of type \"" + typeof(T).Name + "\" loaded in this project. It's called a Singleton for a reason, buddy! Please remove other assets of that type from this project.");
                    }

                    _instance = (objs.Length > 0) ? objs[0] : null;
                }

                return _instance;
            }
        }
    }
}