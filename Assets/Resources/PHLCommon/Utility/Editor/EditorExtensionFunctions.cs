using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PHL.Common.Utility
{
    public static class EditorExtensionFunctions
    {
        public static void SetArrayValue(this Editor obj, string arrayName, int index, object value)
        {
            if (value is bool)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).boolValue = (bool)value;
            }
            else if (value is int)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).intValue = (int)value;
            }
            else if (value is float)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).floatValue = (float)value;
            }
            else if (value is string)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).stringValue = (string)value;
            }
            else if (value is UnityEngine.Object)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).objectReferenceValue = (UnityEngine.Object)value;
            }
            else if (value is Color)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).colorValue = (Color)value;
            }
            else if (value is Enum)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).enumValueIndex = (int)value;
            }
            else if (value is Quaternion)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).quaternionValue = (Quaternion)value;
            }
            else if (value is Vector2)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).vector2Value = (Vector2)value;
            }
            else if (value is Vector3)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).vector3Value = (Vector3)value;
            }
            else if (value is Vector4)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).vector4Value = (Vector4)value;
            }
            else if (value is Vector2Int)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).vector2IntValue = (Vector2Int)value;
            }
            else if (value is Vector3Int)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).vector3IntValue = (Vector3Int)value;
            }
            else if (value is Rect)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).rectValue = (Rect)value;
            }
            else if (value is RectInt)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).rectIntValue = (RectInt)value;
            }
            else if (value is long)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).longValue = (long)value;
            }
            else if (value is double)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).doubleValue = (double)value;
            }
            else if (value is Bounds)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).boundsValue = (Bounds)value;
            }
            else if (value is BoundsInt)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).boundsIntValue = (BoundsInt)value;
            }
            else if (value is AnimationCurve)
            {
                obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).animationCurveValue = (AnimationCurve)value;
            }

            obj.serializedObject.ApplyModifiedProperties();
        }

        public static void InsertArrayValue(this Editor obj, string arrayName, object value, int index)
        {
            obj.serializedObject.FindProperty(arrayName).InsertArrayElementAtIndex(index);
            obj.SetArrayValue(arrayName, index, value);
        }

        public static void AppendArrayValue(this Editor obj, string arrayName, object value)
        {
            int index = obj.serializedObject.FindProperty(arrayName).arraySize;
            obj.InsertArrayValue(arrayName, value, index);
        }

        public static int GetArraySize(this Editor obj, string arrayName)
        {
            return obj.serializedObject.FindProperty(arrayName).arraySize;
        }

        public static SerializedObject GetSerializedObjectFromArray(this Editor obj, string arrayName, int index)
        {
            return obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).serializedObject;
        }

        public static void RemoveObjectAtIndex(this Editor obj, string arrayName, int index)
        {
            obj.serializedObject.FindProperty(arrayName).DeleteArrayElementAtIndex(index);
            obj.serializedObject.FindProperty(arrayName).DeleteArrayElementAtIndex(index);
        }

        public static T GetObjectAtIndex<T>(this Editor obj, string arrayName, int index)
        {
            object value = null;

            if (typeof(T) == typeof(bool))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).boolValue;
            }
            else if (typeof(T) == typeof(int))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).intValue;
            }
            else if (typeof(T) == typeof(float))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).floatValue;
            }
            else if (typeof(T) == typeof(string))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).stringValue;
            }
            else if (obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).propertyType == SerializedPropertyType.ObjectReference)
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).objectReferenceValue;
            }
            else if (typeof(T) == typeof(Color))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).colorValue;
            }
            else if (typeof(T) == typeof(int))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).enumValueIndex;
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).quaternionValue;
            }
            else if (typeof(T) == typeof(Vector2))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).vector2Value;
            }
            else if (typeof(T) == typeof(Vector3))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).vector3Value;
            }
            else if (typeof(T) == typeof(Vector4))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).vector4Value;
            }
            else if (typeof(T) == typeof(Vector2Int))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).vector2IntValue;
            }
            else if (typeof(T) == typeof(Vector3Int))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).vector3IntValue;
            }
            else if (typeof(T) == typeof(Rect))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).rectValue;
            }
            else if (typeof(T) == typeof(RectInt))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).rectIntValue;
            }
            else if (typeof(T) == typeof(long))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).longValue;
            }
            else if (typeof(T) == typeof(double))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).doubleValue;
            }
            else if (typeof(T) == typeof(Bounds))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).boundsValue;
            }
            else if (typeof(T) == typeof(BoundsInt))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).boundsIntValue;
            }
            else if (typeof(T) == typeof(AnimationCurve))
            {
                value = obj.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, index)).animationCurveValue;
            }

            return (T)value;// Convert.ChangeType(value, typeof(T));
        }

        private static UnityEngine.Object _sourceScript;
        
        [MenuItem("CONTEXT/MonoBehaviour/Copy Values With Inheritence")]
        private static void CopyComponentValues(MenuCommand command)
        {
            _sourceScript = command.context;
        }

        [MenuItem("CONTEXT/MonoBehaviour/Paste Values With Inheritence")]
        private static void PasteComponentValues(MenuCommand command)
        {
            UnityEngine.Object oldObject = _sourceScript;
            UnityEngine.Object newObject = command.context;

            foreach (FieldInfo info in oldObject.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (info.IsPublic || info.GetCustomAttributes(typeof(SerializeField), true).Length != 0)
                {
                    info.SetValue(newObject, info.GetValue(oldObject));
                }
            }

            EditorUtility.SetDirty(newObject);
        }

        [MenuItem("Tools/PHL/Count Selected Objects")]
        private static void CountSelectedObjects()
        {
            Debug.Log("Objects selected: " + Selection.objects.Length);
        }

        [MenuItem("Tools/PHL/Clear PlayerPrefs")]
        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Player prefs cleared!");
        }

        [MenuItem("Tools/PHL/Measure Distance")]
        private static void MeasureDistance()
        {
            if(Selection.objects.Length != 2)
            {
                Debug.Log("You must select exactly 2 objects to measure the distance between them!");
                return;
            }

            foreach(UnityEngine.Object obj in Selection.objects)
            {
                if(!(obj is GameObject))
                {
                    Debug.Log("You must only select gameobjects!");
                    return;
                }
            }

            GameObject obj1 = (GameObject)(Selection.objects[0]);
            GameObject obj2 = (GameObject)(Selection.objects[1]);

            Debug.Log(Vector3.Distance(obj1.transform.position, obj2.transform.position));
        }
    }
}