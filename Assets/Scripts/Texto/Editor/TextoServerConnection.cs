using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using TinyJSON;
using PHL.Common.Utility;
using System;
using ArabicSupport;

namespace PHL.Texto
{
    public class TextoServerConnection
    {
        [System.Serializable]
        private class GoogleSheetsJSON
        {
            public string range;
            public string majorDimension;
            public string[][] values;
        }

        [MenuItem("Tools/PHL/Texto/Pull Textos From Google Docs")]
        public static void PullLatestFromGoogleDocs()
        {
            PullLatestFromGoogleDocs(TextoSettingsData.instance);
        }

        public static void PullLatestFromGoogleDocs(TextoSettingsData textoSettingsData)
        {
            EditorCoroutineRunner.StartCoroutine(PullRoutine(textoSettingsData));
        }

        private static IEnumerator PullRoutine(TextoSettingsData textoSettingsData)
        {
            EditorUtility.DisplayProgressBar("Pulling Textos!", "Please wait...", 0);
            
            string range = "A1:Z2000";

            foreach (TextoSettingsData.GoogleSheetInfo googleSheetInfo in textoSettingsData.googleSheetsInfos)
            {
                foreach (string tableID in googleSheetInfo.tableIDs)
                {
                    EditorUtility.DisplayProgressBar("Pulling Textos!", "Please wait...", ExtraMath.Map(googleSheetInfo.tableIDs.IndexOf(tableID), 0, googleSheetInfo.tableIDs.Count, 0.05f, 1));

                    string url = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}/values/{1}!{2}?key={3}", googleSheetInfo.sheetID, tableID, range, TextoSettingsData.instance.googleAPIKey);

                    using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
                    {
                        yield return webRequest.SendWebRequest();

                        try
                        {
                            string[] pages = url.Split('/');
                            int page = pages.Length - 1;

                            if (webRequest.isNetworkError)
                            {
                                Debug.Log(pages[page] + ": Error: " + webRequest.error);
                            }
                            else
                            {
                                Variant jsonVar = Decoder.Decode(webRequest.downloadHandler.text);
                                GoogleSheetsJSON textoJSON = new GoogleSheetsJSON();
                                jsonVar.Make(out textoJSON);

                                int labelsRow = 0;
                                List<string> labelsRowStrings = new List<string>(textoJSON.values[labelsRow]);

                                int uniqueIDColumn = labelsRowStrings.FindIndex(x => x.ToLower() == "line");

                                if (uniqueIDColumn < 0)
                                {
                                    uniqueIDColumn = labelsRowStrings.FindIndex(x => x.ToLower() == "uniqueid");
                                }

                                if (uniqueIDColumn >= 0)
                                {
                                    Dictionary<TextoLanguage, int> languageColumns = new Dictionary<TextoLanguage, int>();

                                    foreach (TextoLanguage language in TextoSettingsData.instance.languages)
                                    {
                                        int index = labelsRowStrings.FindIndex(x => Texto.StringToLanguage(x) == language);
                                        languageColumns.Add(language, index);
                                    }

                                    string[] paths = AssetDatabase.FindAssets("t:TextoData");

                                    List<TextoData> allTextos = new List<TextoData>();

                                    for (int i = 0; i < paths.Length; i++)
                                    {
                                        allTextos.Add(AssetDatabase.LoadAssetAtPath<TextoData>(AssetDatabase.GUIDToAssetPath(paths[i])));
                                    }

                                    for (int i = labelsRow + 1; i < textoJSON.values.Length; i++)
                                    {
                                        if (textoJSON.values.Length <= i)
                                        {
                                            continue;
                                        }

                                        if (textoJSON.values[i].Length <= uniqueIDColumn)
                                        {
                                            continue;
                                        }

                                        string uniqueID = textoJSON.values[i][uniqueIDColumn];
                                        
                                        if (!string.IsNullOrEmpty(uniqueID) && !string.IsNullOrWhiteSpace(uniqueID))
                                        {
                                            TextoData matchedData = allTextos.Find(x => x.uniqueID.ToLower() == uniqueID.ToLower());

                                            if (matchedData == null)
                                            {
                                                Debug.Log("Creating new Texto with ID: " + uniqueID);
                                                TextoData newTexto = ScriptableObject.CreateInstance<TextoData>();
                                                
                                                string parentFolderPath = TextoSettingsData.instance.folderPath;

                                                if (!AssetDatabase.IsValidFolder(parentFolderPath))
                                                {
                                                    Debug.Log(string.Format("Folder \"{0}\" doesn't exist!", parentFolderPath));
                                                    EditorUtility.ClearProgressBar();
                                                    yield break;
                                                }
                                                
                                                string folderPath = parentFolderPath + "/" + tableID;

                                                if (!AssetDatabase.IsValidFolder(folderPath))
                                                {
                                                    AssetDatabase.CreateFolder(parentFolderPath, tableID);
                                                }

                                                AssetDatabase.CreateAsset(newTexto, folderPath + "/" + uniqueID + ".asset");
                                                matchedData = newTexto;

                                                SerializedObject newSerializedObject = new SerializedObject(matchedData);
                                                newSerializedObject.FindProperty("uniqueID").stringValue = uniqueID;
                                                newSerializedObject.ApplyModifiedProperties();
                                            }

                                            SerializedObject serializedObject = new SerializedObject(matchedData);

                                            foreach (TextoLanguage language in TextoSettingsData.instance.languages)
                                            {
                                                int languageColumn = languageColumns[language];
                                                string languageText = "[Not found]";

                                                if (languageColumn >= 0)
                                                {
                                                    if (languageColumn < textoJSON.values[i].Length)
                                                    {
                                                        if (language == TextoLanguage.Arabic)
                                                        {
                                                            languageText = ArabicFixer.Fix(textoJSON.values[i][languageColumn]);
                                                        }
                                                        else
                                                        {
                                                            languageText = textoJSON.values[i][languageColumn];
                                                        }
                                                    }
                                                }

                                                int lineIndex = -1;

                                                for (int j = 0; j < matchedData.lines.Count; j++)
                                                {
                                                    if (matchedData.lines[j].language == language)
                                                    {
                                                        lineIndex = j;
                                                        break;
                                                    }
                                                }

                                                if (lineIndex >= 0)
                                                {
                                                    SerializedProperty lineProperty = serializedObject.FindProperty(string.Format("lines.Array.data[{0}]", lineIndex));
                                                    lineProperty.FindPropertyRelative("text").stringValue = languageText;
                                                    serializedObject.ApplyModifiedProperties();
                                                }
                                                else
                                                {
                                                    SerializedProperty linesProperty = serializedObject.FindProperty("lines");

                                                    int newIndex = matchedData.lines.Count;

                                                    linesProperty.InsertArrayElementAtIndex(newIndex);

                                                    SerializedProperty lineProperty = serializedObject.FindProperty(string.Format("lines.Array.data[{0}]", newIndex));
                                                    lineProperty.FindPropertyRelative("text").stringValue = languageText;
                                                    lineProperty.FindPropertyRelative("language").enumValueIndex = (int)language;
                                                    serializedObject.ApplyModifiedProperties();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch(Exception e)
                        {
                            EditorUtility.ClearProgressBar();
                            Debug.LogException(e);
                            yield break;
                        }
                    }
                }
            }

            EditorUtility.ClearProgressBar();
        }

        [MenuItem("Tools/PHL/Texto/Print Characters/Simplified Chinese")]
        public static void PrintChineseCharacters()
        {
            Debug.Log(String.Join("", PrintCharacters(new List<char>(), TextoLanguage.SimplifiedChinese)));
        }

        [MenuItem("Tools/PHL/Texto/Print Characters/Russian")]
        public static void PrintRussianCharacters()
        {
            Debug.Log(String.Join("", PrintCharacters(new List<char>(), TextoLanguage.Russian)));
        }

        [MenuItem("Tools/PHL/Texto/Print Characters/Japanese")]
        public static void PrintJapaneseCharacters()
        {
            Debug.Log(String.Join("", PrintCharacters(new List<char>(), TextoLanguage.Japanese)));
        }

        [MenuItem("Tools/PHL/Texto/Print Characters/Arabic")]
        public static void PrintArabicCharacters()
        {
            Debug.Log(String.Join("", PrintCharacters(new List<char>(), TextoLanguage.Arabic)));
        }

        [MenuItem("Tools/PHL/Texto/Print Characters/Polish")]
        public static void PrintPolishCharacters()
        {
            Debug.Log(String.Join("", PrintCharacters(new List<char>(), TextoLanguage.Polish)));
        }

        [MenuItem("Tools/PHL/Texto/Print Characters/Latin")]
        public static void PrintLatinCharacters()
        {
            List<char> allLanguages = new List<char>();

            allLanguages = PrintCharacters(allLanguages, TextoLanguage.English);
            allLanguages = PrintCharacters(allLanguages, TextoLanguage.French);
            allLanguages = PrintCharacters(allLanguages, TextoLanguage.Italian);
            allLanguages = PrintCharacters(allLanguages, TextoLanguage.German);
            allLanguages = PrintCharacters(allLanguages, TextoLanguage.Spanish);
            allLanguages = PrintCharacters(allLanguages, TextoLanguage.LatinSpanish);
            allLanguages = PrintCharacters(allLanguages, TextoLanguage.BrazilianPortuguese);
            allLanguages = PrintCharacters(allLanguages, TextoLanguage.Polish);

            Debug.Log(String.Join("", allLanguages));
        }

        public static List<Char> PrintCharacters(List<char> uniqueCharacters, TextoLanguage language)
        {
            string[] paths = AssetDatabase.FindAssets("t:TextoData");
            List<TextoData> allTextos = new List<TextoData>();

            for (int i = 0; i < paths.Length; i++)
            {
                allTextos.Add(AssetDatabase.LoadAssetAtPath<TextoData>(AssetDatabase.GUIDToAssetPath(paths[i])));
            }

            foreach (TextoData t in allTextos)
            {
                string l = t.GetLine(language);

                for (int i = 0; i < l.Length; i++)
                {
                    if (!uniqueCharacters.Contains(l[i]))
                    {
                        uniqueCharacters.Add(l[i]);
                    }
                }

                if (language == TextoLanguage.Arabic)
                {
                    l = ArabicFixer.Fix(l);
                    for (int i = 0; i < l.Length; i++)
                    {
                        if (!uniqueCharacters.Contains(l[i]))
                        {
                            uniqueCharacters.Add(l[i]);
                        }
                    }
                }
            }

            return uniqueCharacters;
        }
    }
}