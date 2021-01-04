using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutableScriptableObject : ScriptableObject
{
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
}