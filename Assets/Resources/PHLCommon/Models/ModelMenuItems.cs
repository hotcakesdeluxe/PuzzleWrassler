using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PHL.Common.Utility
{
    public class MenuItems
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/3D Object/Monkey")]
        private static void AddSuzanne()
        {
            GameObject suzanne = new GameObject("Suzanne");
            suzanne.AddComponent<MeshFilter>();
            suzanne.AddComponent<MeshRenderer>();

            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/PHLCommon/Models/Suzanne.fbx");

            suzanne.GetComponent<MeshFilter>().sharedMesh = mesh;
            suzanne.GetComponent<MeshRenderer>().material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");
        }
#endif
    }
}