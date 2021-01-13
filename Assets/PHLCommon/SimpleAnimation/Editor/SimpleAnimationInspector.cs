using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimplerAnimation))]
public class SimpleAnimationInspector : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty playOnStartProperty = serializedObject.FindProperty("_playOnStart");
        SerializedProperty loopProperty = serializedObject.FindProperty("_loop");
        SerializedProperty randomizeProperty = serializedObject.FindProperty("_randomizeStartTime");
        SerializedProperty currentTimeProperty = serializedObject.FindProperty("_currentTime");
        SerializedProperty animationObjectProperty = serializedObject.FindProperty("_animationObject");
        SerializedProperty spaceProperty = serializedObject.FindProperty("_space");
        SerializedProperty motionType = serializedObject.FindProperty("_motionType");
        SerializedProperty animatePositionProperty = serializedObject.FindProperty("_animatePosition");
        SerializedProperty startingPositionProperty = serializedObject.FindProperty("_startingPosition");
        SerializedProperty endingPositionProperty = serializedObject.FindProperty("_endingPosition");
        SerializedProperty animateRotationProperty = serializedObject.FindProperty("_animateRotation");
        SerializedProperty startingRotationProperty = serializedObject.FindProperty("_startingRotation");
        SerializedProperty endingRotationProperty = serializedObject.FindProperty("_endingRotation");
        SerializedProperty animateScaleProperty = serializedObject.FindProperty("_animateScale");
        SerializedProperty startingScaleProperty = serializedObject.FindProperty("_startingScale");
        SerializedProperty endingScaleProperty = serializedObject.FindProperty("_endingScale");
        SerializedProperty animationLengthProperty = serializedObject.FindProperty("_animationLength");
        SerializedProperty easingCurveProperty = serializedObject.FindProperty("_easingCurve");

        EditorGUILayout.PropertyField(playOnStartProperty);
        EditorGUILayout.PropertyField(loopProperty);
        EditorGUILayout.PropertyField(randomizeProperty);
        EditorGUILayout.PropertyField(currentTimeProperty);
        EditorGUILayout.PropertyField(animationObjectProperty);
        EditorGUILayout.PropertyField(motionType);
        EditorGUILayout.PropertyField(spaceProperty);
        EditorGUILayout.PropertyField(animationLengthProperty);
        EditorGUILayout.PropertyField(easingCurveProperty);
        EditorGUILayout.PropertyField(animatePositionProperty);

        if (animatePositionProperty.boolValue)
        {
            EditorGUI.indentLevel++;

            if (((SimplerAnimation.MotionType)motionType.enumValueIndex) == SimplerAnimation.MotionType.Absolute)
            {
                EditorGUILayout.PropertyField(startingPositionProperty);
                EditorGUILayout.PropertyField(endingPositionProperty);
            }
            else
            {
                EditorGUILayout.PropertyField(endingPositionProperty, new GUIContent("Position Offset"));
            }

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(animateRotationProperty);

        if(animateRotationProperty.boolValue)
        {
            EditorGUI.indentLevel++;

            if (((SimplerAnimation.MotionType)motionType.enumValueIndex) == SimplerAnimation.MotionType.Absolute)
            {
                EditorGUILayout.PropertyField(startingRotationProperty);
                EditorGUILayout.PropertyField(endingPositionProperty);
            }
            else
            {
                EditorGUILayout.PropertyField(endingRotationProperty, new GUIContent("Rotation Offset"));
            }

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(animateScaleProperty);

        if (animateScaleProperty.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(startingScaleProperty);
            EditorGUILayout.PropertyField(endingScaleProperty);
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
