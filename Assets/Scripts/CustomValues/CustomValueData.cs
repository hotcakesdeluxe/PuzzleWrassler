using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomValue", menuName = "PHL/Custom Value", order = 0)]
public class CustomValueData : ScriptableObject
{
    public enum ValueType
    {
        Bool,
        Int,
        Float,
        String
    }

    public ValueType type;
}
