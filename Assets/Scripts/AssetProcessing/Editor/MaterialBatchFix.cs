using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;

public class MaterialBatchFix : EditorWindow
{
    //gui vars
    private AnimBool _showAssignMaterials;
    private AnimBool _showCreateMaterials;
    private Vector2 scrollPos;
    //not gui vars
    private List<string> fbxMaterials = new List<string>();
    private List<Material> assignableMats = new List<Material>();
    private Dictionary<string, Material> materialToAssign = new Dictionary<string, Material>();
    private Dictionary<string, List<string>> materialModelAssociation = new Dictionary<string, List<string>>();
    private Shader createMatShader;
    private string path = "Assets/Materials/";
    private Object folder = null;

    [MenuItem("Window/PHL/Material Batch Fix")]
    #region gui function
    private static void OpenWindow()
    {
        MaterialBatchFix window = (MaterialBatchFix)GetWindow(typeof(MaterialBatchFix));
        window.minSize = new Vector2(470, 260);
        Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Scripts/AssetProcessing/Editor/Images/materialRepairIcon.png");
        window.titleContent = new GUIContent("Material Girl", icon);
        window.Show();
    }
    private void OnEnable()
    {
        _showAssignMaterials = new AnimBool(true);
        _showAssignMaterials.valueChanged.AddListener(new UnityAction(base.Repaint));
        _showCreateMaterials = new AnimBool(false);
        _showCreateMaterials.valueChanged.AddListener(new UnityAction(base.Repaint));
    }
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        DrawHeader();
        DrawExistMat();
        DrawCreateMat();
        GUILayout.EndVertical();
    }
    private void DrawHeader()
    {
        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            GUI.skin.label.fontSize = 12;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Select FBXs to Assign Materials To");
        }
    }
    private void DrawExistMat()
    {
        _showAssignMaterials.target = EditorGUILayout.BeginFoldoutHeaderGroup(_showAssignMaterials.target, "Assign Existing Material");
        using (var group = new EditorGUILayout.FadeGroupScope(_showAssignMaterials.faded))
        {
            if (group.visible)
            {
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    GUILayout.Space(10);
                    if (GUILayout.Button("Get Materials From Assets", GUILayout.Height(25), GUILayout.Width(Screen.width / 3f)))
                    {
                        fbxMaterials.Clear();
                        assignableMats.Clear();
                        materialToAssign.Clear();
                        materialModelAssociation.Clear();
                        GetSelectedMaterials();
                        fbxMaterials = fbxMaterials.Distinct().ToList();
                        CreateMaterialList();
                    }
                    GUILayout.Label("Materials To Be Replaced: ");

                    float scrollViewHeight = 10;
                    float maxHeight = 165;

                    if (assignableMats != null)
                    {
                        scrollViewHeight = (assignableMats.Count * 20) + 5;
                        scrollViewHeight = Mathf.Min(scrollViewHeight, maxHeight);
                    }

                    using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos, GUILayout.Height(scrollViewHeight)))
                    {
                        scrollPos = scrollView.scrollPosition;
                        if (assignableMats != null)
                        {
                            EditorGUI.BeginChangeCheck();
                            for (int i = 0; i < assignableMats.Count; i++)
                            {
                                string tooltip ="";
                                if(materialModelAssociation.ContainsKey(fbxMaterials[i]))
                                {
                                    tooltip = string.Join<string>(", ", materialModelAssociation[fbxMaterials[i]]);
                                }
                                assignableMats[i] = EditorGUILayout.ObjectField(new GUIContent(fbxMaterials[i], tooltip), assignableMats[i], typeof(Material), false) as Material;
                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                for (int i = 0; i < assignableMats.Count; i++)
                                {
                                    if (!materialToAssign.ContainsKey(fbxMaterials[i]))
                                    {
                                        materialToAssign.Add(fbxMaterials[i], assignableMats[i]);
                                    }
                                    else
                                    {
                                        materialToAssign[fbxMaterials[i]] = assignableMats[i];
                                    }
                                }
                            }
                        }
                    }

                    if (GUILayout.Button("Assign Materials", GUILayout.Height(25), GUILayout.Width(Screen.width / 3f)))
                    {
                        if (!ValidateSelection())
                        {
                            Debug.Log("Please Select a Model To Assign Materials To");
                            return;
                        }
                        RemapMaterials();
                    }
                }
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
    private void DrawCreateMat()
    {
        _showCreateMaterials.target = EditorGUILayout.BeginFoldoutHeaderGroup(_showCreateMaterials.target, "Create Material to Assign");
        using (var group = new EditorGUILayout.FadeGroupScope(_showCreateMaterials.faded))
        {
            if (group.visible)
            {
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    path = EditorPrefs.GetString("MaterialBatchFix.MaterialsFolder");
                    GUILayout.BeginHorizontal();
                    if (DragAndDrop.objectReferences.Length > 0)
                    {
                        folder = EditorGUILayout.ObjectField("Material Folder", folder, typeof(Object), false, GUILayout.Width(300));
                    }
                    else
                    {
                        EditorGUILayout.TextField("Material Folder", path);
                        if (GUILayout.Button("...", GUILayout.Width(30)))
                        {
                            path = EditorUtility.OpenFolderPanel("Materials folder", "", "");
                        }
                    }

                    if (folder != null)
                    {
                        path = AssetDatabase.GetAssetPath(folder);
                    }

                    EditorPrefs.SetString("MaterialBatchFix.MaterialsFolder", path);
                    GUILayout.EndHorizontal();
                    createMatShader = (Shader)EditorGUILayout.ObjectField("Gimme Shader", createMatShader, typeof(Shader), false);
                    string matName = "New Material";
                    EditorGUILayout.TextField("Material Name", matName);
                    if (GUILayout.Button("Create and Assign", GUILayout.Height(25), GUILayout.Width(Screen.width / 3f)))
                    {
                        fbxMaterials.Clear();
                        assignableMats.Clear();
                        materialToAssign.Clear();
                        materialModelAssociation.Clear();
                        GetSelectedMaterials();
                        fbxMaterials = fbxMaterials.Distinct().ToList();
                        CreateMaterialList();
                        if (!ValidateSelection())
                        {
                            Debug.Log("Please Select a Model To Assign Materials To");
                            return;
                        }
                        Material newMat = CreateMaterial(createMatShader);
                        AssetDatabase.CreateAsset(newMat, path + "/" + matName + ".mat");
                        materialToAssign.Add(fbxMaterials[0], newMat);
                        RemapMaterials();
                    }
                }
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
    #endregion
    #region 
    private static bool ValidateSelection()
    {
        return Selection.activeObject != null;
    }

    private void CreateMaterialList()
    {
        foreach (string material in fbxMaterials)
        {
            Material tempmat = null;
            assignableMats.Add(tempmat);
        }
    }

    private void GetSelectedMaterials()
    {
        if (ValidateSelection())
        {
            foreach (GameObject obj in Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets))
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ModelImporter modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;
                var sourceMaterials = typeof(ModelImporter).GetProperty("sourceMaterials", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(modelImporter) as AssetImporter.SourceAssetIdentifier[];
                foreach (var identifier in sourceMaterials ?? Enumerable.Empty<AssetImporter.SourceAssetIdentifier>())
                {
                    fbxMaterials.Add(identifier.name);
                    
                    if(materialModelAssociation.ContainsKey(identifier.name))
                    {
                        materialModelAssociation[identifier.name].Add(obj.name);
                    }
                    else
                    {
                        materialModelAssociation.Add(identifier.name, new List<string>{obj.name});
                    }
                }
                
            }
        }
    }
    private void RemapMaterials()
    {
        if (ValidateSelection())
        {
            foreach (GameObject obj in Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets))
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ModelImporter modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;
                modelImporter.materialImportMode = ModelImporterMaterialImportMode.ImportViaMaterialDescription;
                modelImporter.materialLocation = ModelImporterMaterialLocation.InPrefab;
                modelImporter.materialName = ModelImporterMaterialName.BasedOnModelNameAndMaterialName;
                modelImporter.materialSearch = ModelImporterMaterialSearch.Local;
                var sourceMaterials = typeof(ModelImporter).GetProperty("sourceMaterials", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(modelImporter) as AssetImporter.SourceAssetIdentifier[];
                foreach (var identifier in sourceMaterials ?? Enumerable.Empty<AssetImporter.SourceAssetIdentifier>())
                {

                    if (materialToAssign.TryGetValue(identifier.name, out Material result))
                    {
                        modelImporter.AddRemap(identifier, result);
                    }
                    else
                    {
                        string newPath = path.Replace(obj.name + ".fbx", "");
                        newPath += "Materials/" + obj.name + "-" + identifier.name + ".mat";
                        modelImporter.materialLocation = ModelImporterMaterialLocation.External;
                        SaveReimport();
                        modelImporter.materialLocation = ModelImporterMaterialLocation.InPrefab;
                        Material extractedMat = (Material)AssetDatabase.LoadAssetAtPath(newPath, typeof(Material));
                        modelImporter.AddRemap(identifier, extractedMat);
                        Debug.Log("Some Materials Were Not Assigned. Extracting Material from FBX at " + newPath);
                    }
                }
                SaveReimport();
            }
        }
    }
    private Material CreateMaterial(Shader shader)
    {
        Material material = new Material(shader);
        return material;
    }

    private void SaveReimport()
    {
        if (ValidateSelection())
        {
            foreach (GameObject obj in Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets))
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ModelImporter modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;
                SerializedObject modelImporterObj = new SerializedObject(modelImporter);
                modelImporterObj.ApplyModifiedProperties();
                modelImporter.SaveAndReimport();
            }
        }
    }
    #endregion
}
