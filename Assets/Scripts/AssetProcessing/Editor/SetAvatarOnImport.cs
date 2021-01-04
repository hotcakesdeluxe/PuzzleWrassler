using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetAvatarOnImport : AssetPostprocessor
{
    void OnPreprocessAnimation()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        string[] filename = null;
        string name = "";
        string avatarPath = "";

        string assetPathStr = modelImporter.assetPath;
        string[] lines = assetPathStr.Split('/');
        if (assetPathStr.Contains("Assets/Animations") && !assetPathStr.ToLower().Contains("debug"))
        {
            if (modelImporter.avatarSetup == ModelImporterAvatarSetup.NoAvatar)
            {
                foreach (string line in lines)
                {
                    if (line.Contains("_"))
                    {
                        filename = line.Split('_');
                        name = filename[0];
                    }
                    else
                    {
                        filename = line.Split('/');
                        name = filename[filename.Length - 1];
                    }
                }
                string[] results;
                results = AssetDatabase.FindAssets(name, new[] { "Assets/Models" });
                if (results.Length == 0)
                {
                    Debug.Log("No corresponding Avatar found for this animation. Make sure a rig is available within the Assets/Models folder. Set to Generic for now.");
                    modelImporter.animationType = ModelImporterAnimationType.Generic;
                    modelImporter.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
                }
                else
                {
                    avatarPath = AssetDatabase.GUIDToAssetPath(results[0]);
                    Debug.Log("Avatar copied from " + avatarPath);
                    var avatarAsset = AssetDatabase.LoadAssetAtPath<Avatar>(avatarPath);
                    modelImporter.animationType = ModelImporterAnimationType.Generic;
                    modelImporter.avatarSetup = ModelImporterAvatarSetup.CopyFromOther;
                    modelImporter.sourceAvatar = avatarAsset;
                }
                //extra
                modelImporter.resampleCurves = false;
                modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
            }
        }
        else
        {
            return;
        }
    }
}
