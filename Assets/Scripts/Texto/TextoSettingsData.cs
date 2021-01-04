using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PHL.Common.Utility;

namespace PHL.Texto
{
    [CreateAssetMenu(fileName = "TextoSettingsData", menuName = "PHL/Singletons/Texto Settings", order = 0)]
    public class TextoSettingsData : ScriptableObjectSingleton<TextoSettingsData>
    {
        [System.Serializable]
        public class GoogleSheetInfo
        {
            public string sheetName;
            public string sheetID;
            public List<string> tableIDs;
        }

        public Color highlightColor;
        public string folderPath = "Assets/ScriptableObjects/Textos";
        public List<TextoLanguage> languages;
        public string googleAPIKey;
        public List<GoogleSheetInfo> googleSheetsInfos;
    }
}