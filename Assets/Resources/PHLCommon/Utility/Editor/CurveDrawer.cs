using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CurveAttribute))]
public class CurveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CurveAttribute curve = attribute as CurveAttribute;
        if (property.propertyType == SerializedPropertyType.AnimationCurve)
        {
            Vector2 pos = Vector2.zero;
            Vector2 range = Vector2.one;

            /*foreach(Keyframe keyframe in property.animationCurveValue.keys)
            {
                if(keyframe.)
            }*/

            EditorGUI.CurveField(position, property, curve.color, new Rect(pos.x, pos.y, range.x, range.y));
        }
    }
}