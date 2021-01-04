using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PositionData))]
public class PositionDataEditor : Editor
{
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        PositionData position = target as PositionData;
        
        Camera sceneCamera = SceneView.lastActiveSceneView.camera;
        if (sceneCamera == null)
        {
            return;
        }

        Event e = Event.current;

        if (position != null)
        {
            SerializedProperty positionProperty = serializedObject.FindProperty("position");
            SerializedProperty angle = serializedObject.FindProperty("angle");

            Handles.color = Color.magenta;

            //Just to make a circle
            Handles.Button(position.position, Quaternion.Euler(90, 0, 0), 0.5f, 0.0f, Handles.CircleHandleCap);
            Vector3 angledPosition = positionProperty.vector3Value + new Vector3(Mathf.Sin(angle.floatValue * Mathf.Deg2Rad), 0, Mathf.Cos(angle.floatValue * Mathf.Deg2Rad)) * 2;
            Handles.DrawLine(position.position, angledPosition);

            if (Tools.current == Tool.Move || Tools.current == Tool.Scale)
            {
                positionProperty.vector3Value = Handles.PositionHandle(positionProperty.vector3Value, Quaternion.Euler(0, angle.floatValue, 0));
            }
            else if(Tools.current == Tool.Rotate)
            {
                Quaternion newRot = Quaternion.Euler(0, angle.floatValue, 0);
                newRot = Handles.RotationHandle(newRot, positionProperty.vector3Value);
                angle.floatValue = newRot.eulerAngles.y;
            }
            
            if (e != null)
            {
                if (e.type == EventType.KeyDown)
                {
                    if (e.keyCode == KeyCode.F)
                    {
                        sceneView.Frame(new Bounds(position.position, Vector3.one * 2.5f));
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
