using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PHL.Common.Utility
{
    public class LogsterUtility
    {
        public static void SaveSettings(Logster.LogsterSettings settings)
        {
#if UNITY_EDITOR
            EditorPrefs.SetString("PHL.Logster.Data." + Application.dataPath, JsonUtility.ToJson(settings));
#endif
        }

        public static Logster.LogsterSettings LoadSettings()
        {
#if UNITY_EDITOR
            string serializedData = EditorPrefs.GetString("PHL.Logster.Data." + Application.dataPath, "");

            if (string.IsNullOrEmpty(serializedData))
            {
                return new Logster.LogsterSettings();
            }

            return JsonUtility.FromJson<Logster.LogsterSettings>(serializedData);
#else
            return null;
#endif
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitialize()
        {
#if UNITY_EDITOR
            if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.LinuxEditor)
            {
                Logster.settings = LoadSettings();
            }
#endif
        }
    }
}