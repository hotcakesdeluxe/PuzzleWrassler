using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;


public class Prefaber : EditorWindow
{
    //nongui vars
    private static int selectedLayer = 0;
    private static string selectedTag = "Untagged";
    private string path = "Assets/Prefabs/";
    private bool isStatic = false;
    private bool isVariant = false;
    private bool makeParent = false;
    private GameObject parentObj;
    private Object folder = null;
    
    [MenuItem("Window/PHL/Prefaber")]
    #region gui function
    private static void OpenWindow()
    {
        Prefaber window = (Prefaber)GetWindow(typeof(Prefaber));
        window.minSize = new Vector2(500, 160);
        Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Scripts/AssetProcessing/Editor/Images/prefaberIcon.png");
        window.titleContent = new GUIContent("Prefaber", icon);
        window.Show();
    }
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        DrawHeader();
        DrawMain();
        GUILayout.EndVertical();
    }
    private void DrawHeader()
    {
        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            GUI.skin.label.fontSize = 12;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Select Assets to Prefab");
        }
    }
    private void DrawMain()
    {
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        using (new GUILayout.VerticalScope())
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                path = EditorPrefs.GetString("Prefaber.PrefabFolder");
                GUILayout.BeginHorizontal();
                if (DragAndDrop.objectReferences.Length > 0)
                {
                    folder = EditorGUILayout.ObjectField("Prefab Folder", folder, typeof(Object), false, GUILayout.Width(300));
                }
                else
                {
                    EditorGUILayout.TextField("Prefab Folder", path);
                    if (GUILayout.Button("...", GUILayout.Width(30)))
                    {
                        path = EditorUtility.OpenFolderPanel("Prefab folder", "", "");
                    }
                }

                if (folder != null)
                {
                    path = AssetDatabase.GetAssetPath(folder);
                }
                EditorPrefs.SetString("Prefaber.PrefabFolder", path);
                GUILayout.EndHorizontal();
                using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    isStatic = EditorGUILayout.ToggleLeft("Make Prefab Static", isStatic, GUILayout.Width(157));
                    isVariant = EditorGUILayout.ToggleLeft("Make Prefab a Variant", isVariant, GUILayout.Width(157));
                    makeParent = EditorGUILayout.ToggleLeft("Make Parent Object", makeParent, GUILayout.Width(157));
                }
                using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label("Set Prefab Layer");
                    selectedLayer = EditorGUILayout.LayerField(selectedLayer);
                    GUILayout.Label("Set Prefab Tag");
                    selectedTag = EditorGUILayout.TagField(selectedTag);
                }
                if (GUILayout.Button("Make Prefab", GUILayout.Height(25), GUILayout.Width(Screen.width / 3f)))
                {
                    MakePrefabs(path);
                }
            }
        }
    }
    #endregion
    #region 
    private static bool ValidateSelection()
    {
        return Selection.activeObject != null;
    }

    private void MakePrefabs(string path)
    {
        string newPath;
        if (ValidateSelection())
        {
            foreach (GameObject obj in Selection.GetFiltered(typeof(GameObject), SelectionMode.Unfiltered))
            {
                //is selection an asset
                string objPath = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(objPath))
                {
                    if (PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Model)
                    {
                        GameObject objInstance = (GameObject)PrefabUtility.InstantiatePrefab(obj);

                        newPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + obj.name + ".prefab");
                        if (!isVariant)
                        {
                            PrefabUtility.UnpackPrefabInstance(objInstance, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
                        }
                        if (makeParent)
                        {
                            parentObj = new GameObject(obj.name);
                            objInstance.transform.SetParent(parentObj.transform);
                            parentObj.layer = selectedLayer;
                            parentObj.isStatic = isStatic;
                            parentObj.tag = selectedTag;
                            newPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + parentObj.name + ".prefab");
                            GameObject parentPrefab = PrefabUtility.SaveAsPrefabAsset(parentObj, newPath);
                            newPath = "";
                            GameObject.DestroyImmediate(parentObj);
                        }
                        else
                        {
                            objInstance.layer = selectedLayer;
                            objInstance.isStatic = isStatic;
                            objInstance.tag = selectedTag;
                            GameObject objPrefab = PrefabUtility.SaveAsPrefabAsset(objInstance, newPath);
                            newPath = "";
                            GameObject.DestroyImmediate(objInstance);
                        }
                    }
                    else if (PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Regular)
                    {
                        isVariant = true;
                    }
                }
                //is selection in scene
                else
                {
                    GameObject g = null;
                    if (!isVariant)
                    {
                        PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
                    }
                    if (makeParent)
                    {
                        parentObj = new GameObject(obj.name);
                        obj.transform.SetParent(parentObj.transform);
                        parentObj.layer = selectedLayer;
                        parentObj.isStatic = isStatic;
                        parentObj.tag = selectedTag;
                        newPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + parentObj.name + ".prefab");
                        g = PrefabUtility.SaveAsPrefabAsset(parentObj, newPath);
                        newPath = "";
                        g = (GameObject)PrefabUtility.InstantiatePrefab(g, parentObj.scene);
                        GameObject.DestroyImmediate(parentObj);
                    }
                    else
                    {
                        obj.layer = selectedLayer;
                        obj.isStatic = isStatic;
                        obj.tag = selectedTag;
                        newPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + obj.name + ".prefab");
                        g = PrefabUtility.SaveAsPrefabAsset(obj, newPath);
                        newPath = "";
                        g = (GameObject)PrefabUtility.InstantiatePrefab(g, obj.scene);
                        GameObject.DestroyImmediate(obj);
                    }
                }
            }
        }
    }
    #endregion
}
