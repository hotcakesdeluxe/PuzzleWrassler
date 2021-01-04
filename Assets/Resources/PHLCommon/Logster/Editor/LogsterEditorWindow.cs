using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

namespace PHL.Common.Utility
{
    public class LogsterEditorWindow : EditorWindow
    {
        [MenuItem("Window/PHL/Logster")]
        private static void OpenWindow()
        {
            Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/PHLCommon/Logster/Editor/Images/LogsterIcon.png");
            LogsterEditorWindow window = GetWindow<LogsterEditorWindow>();
            window.titleContent = new GUIContent("Logster", icon);
            window.Show();
        }

        private void OnEnable()
        {
            Logster.settings = LogsterUtility.LoadSettings();
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            Logster.LogsterSettings newSettings = new Logster.LogsterSettings();

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            newSettings.showAll = GUILayout.Toggle(Logster.settings.showAll, "Show All", EditorStyles.toolbarButton, GUILayout.Width(75));
            newSettings.showUntagged = GUILayout.Toggle(Logster.settings.showUntagged, "Show Untagged", EditorStyles.toolbarButton, GUILayout.Width(100));
            newSettings.displayTags = GUILayout.Toggle(Logster.settings.displayTags, "Display Tags In Logs", EditorStyles.toolbarButton, GUILayout.Width(125));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            newSettings.tags = new List<Logster.LogsterSettings.LogsterTag>();
            
            HashSet<int> indicesToRemove = new HashSet<int>();

            for(int i = 0; i < Logster.settings.tags.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                newSettings.tags.Add(new Logster.LogsterSettings.LogsterTag()
                {
                    tag = Logster.settings.tags[i].tag,
                    enabled = Logster.settings.tags[i].enabled,
                    tagColor = Logster.settings.tags[i].tagColor
                });

                GUILayout.Space(10);
                newSettings.tags[i].enabled = EditorGUILayout.Toggle(Logster.settings.tags[i].enabled, GUILayout.Width(20));
                newSettings.tags[i].tagColor = EditorGUILayout.ColorField(Logster.settings.tags[i].tagColor, GUILayout.Width(40));
                newSettings.tags[i].tag = EditorGUILayout.DelayedTextField(Logster.settings.tags[i].tag);

                if(GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                {
                    indicesToRemove.Add(i);
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            if (indicesToRemove.Count > 0)
            {
                for (int i = Logster.settings.tags.Count - 1; i >= 0; i--)
                {
                    if (indicesToRemove.Contains(i))
                    {
                        Logster.settings.tags.RemoveAt(i);
                    }
                }

                LogsterUtility.SaveSettings(Logster.settings);
                Logster.settings = LogsterUtility.LoadSettings();
            }
            else
            {
                if (EditorGUI.EndChangeCheck())
                {
                    LogsterUtility.SaveSettings(newSettings);
                    Logster.settings = LogsterUtility.LoadSettings();
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            if (GUILayout.Button("Add Tag", GUILayout.Height(24)))
            {
                Logster.settings.tags.Add(new Logster.LogsterSettings.LogsterTag()
                {
                    tag = "New Tag",
                    enabled = true,
                    tagColor = Color.black
                });

                LogsterUtility.SaveSettings(Logster.settings);
                Logster.settings = LogsterUtility.LoadSettings();
            }
            GUILayout.Space(30);
            GUILayout.EndHorizontal();
        }
    }
}