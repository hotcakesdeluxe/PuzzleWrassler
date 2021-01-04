using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace PHL.Texto
{
    [CustomPropertyDrawer(typeof(TextoData))]
    class TextoDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = new GUIContent(label.text);
            EditorGUI.BeginProperty(position, label, property);
            
            Rect objectFieldRect = position;
            objectFieldRect.height = 16;

            if (property.objectReferenceValue == null)
            {
                Rect buttonRect = position;
                buttonRect.height = 16;
                buttonRect.x = position.width - 30;
                buttonRect.width = 40;

                if(GUI.Button(buttonRect, "New"))
                {
                    property.objectReferenceValue = CreateNewInstance();
                }

                objectFieldRect.width -= 50;
            }
            else
            {
                Rect labelRect = position;
                labelRect.y += 16;
                labelRect.height = 16;

                TextoData textoData = (TextoData)(property.objectReferenceValue);

                if(textoData.lines == null)
                {
                    textoData.lines = new List<TextoLine>();
                }

                TextoLine englishLine = textoData.lines.Find(x => x.language == TextoLanguage.English);

                if (englishLine != null)
                {
                    EditorGUI.LabelField(labelRect, " ", englishLine.text);
                }
                else
                {
                    EditorGUI.LabelField(labelRect, " ", "[No English text]", EditorStyles.boldLabel);
                }
            }
            
            EditorGUI.ObjectField(objectFieldRect, property, this.fieldInfo.FieldType, label);
            
            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue != null)
            {
                return 32;
            }

            return base.GetPropertyHeight(property, label);
        }

        private TextoData CreateNewInstance()
        {
            string path = EditorUtility.SaveFilePanelInProject("Create new Texto Data", "Texto", "asset", "Hello!", TextoSettingsData.instance.folderPath);

            if (path.Length != 0)
            {
                TextoData textoData = ScriptableObject.CreateInstance<TextoData>();

                AssetDatabase.CreateAsset(textoData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                return textoData;
            }

            return null;
        }
    }
}