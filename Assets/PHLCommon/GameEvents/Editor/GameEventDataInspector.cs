using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PHL.Common.GameEvents
{
    [CustomEditor(typeof(GameEventData))]
    public class GameEventDataInspector : Editor
    {
        private GameEventData _gameEventData;

        private void OnEnable()
        {
            _gameEventData = (GameEventData)target;
        }

        public override void OnInspectorGUI()
        {
            if (_gameEventData == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();

            //Bools

            if(_gameEventData.eventBools == null)
            {
                _gameEventData.eventBools = new List<string>();
            }

            if (_gameEventData.eventBools.Count > 0)
            {
                GUILayout.Label("Bools", EditorStyles.boldLabel);
            }

            for (int i = 0; i < _gameEventData.eventBools.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                _gameEventData.eventBools[i] = EditorGUILayout.TextField(_gameEventData.eventBools[i]);
                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(19)))
                {
                    _gameEventData.eventBools.RemoveAt(i);
                    i--;
                    EditorUtility.SetDirty(_gameEventData);
                }
                EditorGUILayout.EndHorizontal();
            }

            //Ints

            if (_gameEventData.eventInts == null)
            {
                _gameEventData.eventInts = new List<string>();
            }

            if (_gameEventData.eventInts.Count > 0)
            {
                GUILayout.Label("Ints", EditorStyles.boldLabel);
            }

            for (int i = 0; i < _gameEventData.eventInts.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                _gameEventData.eventInts[i] = EditorGUILayout.TextField(_gameEventData.eventInts[i]);
                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(19)))
                {
                    _gameEventData.eventInts.RemoveAt(i);
                    i--;
                    EditorUtility.SetDirty(_gameEventData);
                }
                EditorGUILayout.EndHorizontal();
            }

            //Floats

            if (_gameEventData.eventFloats == null)
            {
                _gameEventData.eventFloats = new List<string>();
            }

            if (_gameEventData.eventFloats.Count > 0)
            {
                GUILayout.Label("Floats", EditorStyles.boldLabel);
            }

            for (int i = 0; i < _gameEventData.eventFloats.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                _gameEventData.eventFloats[i] = EditorGUILayout.TextField(_gameEventData.eventFloats[i]);
                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(19)))
                {
                    _gameEventData.eventFloats.RemoveAt(i);
                    i--;
                    EditorUtility.SetDirty(_gameEventData);
                }
                EditorGUILayout.EndHorizontal();
            }

            //Strings

            if (_gameEventData.eventStrings == null)
            {
                _gameEventData.eventStrings = new List<string>();
            }

            if (_gameEventData.eventStrings.Count > 0)
            {
                GUILayout.Label("Strings", EditorStyles.boldLabel);
            }

            for (int i = 0; i < _gameEventData.eventStrings.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                _gameEventData.eventStrings[i] = EditorGUILayout.TextField(_gameEventData.eventStrings[i]);
                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(19)))
                {
                    _gameEventData.eventStrings.RemoveAt(i);
                    i--;
                    EditorUtility.SetDirty(_gameEventData);
                }
                EditorGUILayout.EndHorizontal();
            }

            //Objects

            if (_gameEventData.eventObjects == null)
            {
                _gameEventData.eventObjects = new List<GameEventData.ObjectIDTypePair>();
            }

            if (_gameEventData.eventObjects.Count > 0)
            {
                GUILayout.Label("Objects", EditorStyles.boldLabel);
            }

            for (int i = 0; i < _gameEventData.eventObjects.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                _gameEventData.eventObjects[i].type = (GameEventData.SupportedObjectType)EditorGUILayout.EnumPopup(_gameEventData.eventObjects[i].type, GUILayout.MaxWidth(100));
                _gameEventData.eventObjects[i].objectID = EditorGUILayout.TextField(_gameEventData.eventObjects[i].objectID);
                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(19)))
                {
                    _gameEventData.eventObjects.RemoveAt(i);
                    i--;
                    EditorUtility.SetDirty(_gameEventData);
                }
                EditorGUILayout.EndHorizontal();
            }
            
            //Finishers

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_gameEventData);
            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            if (GUILayout.Button("Add Parameter", GUILayout.Height(22)))
            {
                ShowAddContextMenu();
            }
            GUILayout.Space(30);
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowAddContextMenu()
        {
            GenericMenu contextMenu = new GenericMenu();

            contextMenu.AddItem(new GUIContent("Bool"), false, () =>
            {
                _gameEventData.eventBools.Add("");
                EditorUtility.SetDirty(_gameEventData);
            });

            contextMenu.AddItem(new GUIContent("Int"), false, () =>
            {
                _gameEventData.eventInts.Add("");
                EditorUtility.SetDirty(_gameEventData);
            });

            contextMenu.AddItem(new GUIContent("Float"), false, () =>
            {
                _gameEventData.eventFloats.Add("");
                EditorUtility.SetDirty(_gameEventData);
            });

            contextMenu.AddItem(new GUIContent("String"), false, () =>
            {
                _gameEventData.eventStrings.Add("");
                EditorUtility.SetDirty(_gameEventData);
            });

            contextMenu.AddItem(new GUIContent("Object"), false, () =>
            {
                _gameEventData.eventObjects.Add(new GameEventData.ObjectIDTypePair());
                EditorUtility.SetDirty(_gameEventData);
            });
            
            contextMenu.ShowAsContext();
        }
    }
}