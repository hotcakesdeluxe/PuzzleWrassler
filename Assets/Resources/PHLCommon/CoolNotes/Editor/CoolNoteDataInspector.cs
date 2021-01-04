using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using PHL.Common.Utility;

[CustomEditor(typeof(CoolNoteData))]
public class CoolNoteDataInspector : Editor
{
    private static CoolNoteImageData _editingImage;

    public override void OnInspectorGUI()
    {
        List<Object> components = new List<Object>();

        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(target));
        for (int i = 0; i < assets.Length; i++)
        {
            if (AssetDatabase.IsSubAsset(assets[i]))
            {
                components.Add(assets[i]);
            }
        }

        components.Sort((x, y) => x.name.CompareTo(y.name));

        for (int i = 0; i < components.Count; i++)
        {
            Object componentData = components[i];

            if (componentData is CoolNoteTextData)
            {
                GUILayout.Space(16);
                CoolNoteTextData textData = (CoolNoteTextData)componentData;
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    DeleteComponent(i);
                    break;
                }
                GUILayout.EndHorizontal();
                
                SerializedObject serObj = new SerializedObject(textData);
                serObj.FindProperty("text").stringValue = EditorGUILayout.TextArea(serObj.FindProperty("text").stringValue, GUILayout.MinHeight(30), GUILayout.ExpandHeight(true));
                serObj.ApplyModifiedProperties();
            }
            else if(componentData is CoolNoteImageData)
            {
                GUILayout.Space(16);
                CoolNoteImageData imageData = (CoolNoteImageData)componentData;

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("E", GUILayout.Width(20)))
                {
                    _editingImage = imageData;
                }
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    DeleteComponent(i);
                    break;
                }
                GUILayout.EndHorizontal();

                SerializedObject serObj = new SerializedObject(imageData);

                float width = EditorGUIUtility.currentViewWidth;
                float height = ExtraMath.Map(imageData.image.height, 0, imageData.image.width, 0, width);

                Rect rect = GUILayoutUtility.GetRect(width, height);
                
                GUI.DrawTexture(rect, imageData.image);
                
                serObj.ApplyModifiedProperties();
            }
        }

        serializedObject.ApplyModifiedProperties();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Text"))
        {
            AddTextComponent();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Add Image"))
        {
            AddImageComponent();
        }
        GUILayout.EndHorizontal();
    }
    
    public void AddTextComponent()
    {
        List<Object> components = new List<Object>();

        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(target));
        for (int i = 0; i < assets.Length; i++)
        {
            if (AssetDatabase.IsSubAsset(assets[i]))
            {
                if (assets[i] is CoolNoteTextData)
                {
                    components.Add(assets[i]);
                }
                else if (assets[i] is CoolNoteImageData)
                {
                    components.Add(assets[i]);
                }
            }
        }
        
        int currentCount = components.Count;

        CoolNoteTextData newCoolNoteText = CreateInstance<CoolNoteTextData>();
        newCoolNoteText.name = string.Format("{0}_Text", currentCount.ToString("D8"));
        AssetDatabase.AddObjectToAsset(newCoolNoteText, target);
        
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public void AddImageComponent()
    {
        List<Object> components = new List<Object>();

        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(target));
        for (int i = 0; i < assets.Length; i++)
        {
            if (AssetDatabase.IsSubAsset(assets[i]))
            {
                if (assets[i] is CoolNoteTextData)
                {
                    components.Add(assets[i]);
                }
                else if (assets[i] is CoolNoteImageData)
                {
                    components.Add(assets[i]);
                }
            }
        }

        int currentCount = components.Count;

        CoolNoteImageData newCoolNoteImage = CreateInstance<CoolNoteImageData>();
        newCoolNoteImage.name = string.Format("{0}_Image", currentCount.ToString("D8"));
        AssetDatabase.AddObjectToAsset(newCoolNoteImage, target);

        SerializedObject serObj = new SerializedObject(newCoolNoteImage);

        int width = 500;
        int height = 300;

        Texture2D newTexture = new Texture2D(width, height);

        Color[] colors = new Color[width * height];
        for(int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }

        newTexture.SetPixels(colors);
        newTexture.Apply();

        newTexture.name = "Image";
        AssetDatabase.AddObjectToAsset(newTexture, target);
        serObj.FindProperty("image").objectReferenceValue = newTexture;

        serObj.ApplyModifiedProperties();

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public void DeleteComponent(int index)
    {
        //Delete Component
        List<Object> components = new List<Object>();

        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(target));
        for (int i = 0; i < assets.Length; i++)
        {
            if (AssetDatabase.IsSubAsset(assets[i]))
            {
                components.Add(assets[i]);
            }
        }

        components.Sort((x, y) => x.name.CompareTo(y.name));

        for (int i = 0; i < components.Count; i++)
        {
            if(i == index)
            {
                if(components[i] is CoolNoteImageData)
                {
                    CoolNoteImageData imageData = (CoolNoteImageData)(components[i]);
                    DestroyImmediate(imageData.image, true);
                }

                DestroyImmediate(components[i], true);
                break;
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(target));
        AssetDatabase.Refresh();

        //Rename components
        string path = AssetDatabase.GetAssetPath(target);
        assets = AssetDatabase.LoadAllAssetsAtPath(path);
        
        components.Clear();

        for (int i = 0; i < assets.Length; i++)
        {
            if (AssetDatabase.IsSubAsset(assets[i]))
            {
                if (assets[i] is CoolNoteComponentData)
                {
                    components.Add(assets[i]);
                }
            }
        }

        components.Sort((x, y) => x.name.CompareTo(y.name));

        for(int i = 0; i < components.Count; i++)
        {
            if (components[i] is CoolNoteTextData)
            {
                components[i].name = string.Format("{0}_Text", i.ToString("D8"));
            }
            else if(components[i] is CoolNoteImageData)
            {
                components[i].name = string.Format("{0}_Image", i.ToString("D8"));
            }

            EditorUtility.SetDirty(assets[i]);
        }

        EditorUtility.SetDirty(target);
        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
