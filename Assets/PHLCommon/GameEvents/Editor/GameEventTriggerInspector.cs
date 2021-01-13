using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PHL.Common.GameEvents
{
    [CustomPropertyDrawer(typeof(GameEventTrigger))]
    class IngredientDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position.height = 16;
            Rect lineRect = new Rect(position.x, position.y + 5, position.width, 16);

            if (property.FindPropertyRelative("gameEventData").objectReferenceValue != null)
            {
                Rect totalRect = position;
                totalRect.height = GetPropertyHeight(property, new GUIContent());
                totalRect.x -= 2;
                totalRect.width += 4;

                GUI.Box(totalRect, "", EditorStyles.helpBox);
            }

            Rect valueRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if (property.FindPropertyRelative("gameEventData").objectReferenceValue != null)
            {
                valueRect.y += 5;
            }

            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("gameEventData"), GUIContent.none);

            if (property.FindPropertyRelative("gameEventData").objectReferenceValue != null)
            {
                valueRect.y += 18;
                lineRect.y += 18;
                EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("delay"));

                valueRect.y += 18;
                lineRect.y += 18;
                EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("delayType"));

                SerializedObject gameEventObject = new SerializedObject(property.FindPropertyRelative("gameEventData").objectReferenceValue);
                
                //Objects
                int numObjects = gameEventObject.FindProperty("eventObjects").arraySize;
                property.FindPropertyRelative("objects").arraySize = numObjects;

                if (numObjects > 0)
                {
                    lineRect.y += 18;
                    EditorGUI.LabelField(lineRect, "    Objects", EditorStyles.boldLabel);
                }

                for (int i = 0; i < numObjects; i++)
                {
                    lineRect.y += 18;

                    SerializedProperty objectNameProperty = gameEventObject.FindProperty(string.Format("eventObjects.Array.data[{0}].objectID", i));
                    SerializedProperty objectTypeProperty = gameEventObject.FindProperty(string.Format("eventObjects.Array.data[{0}].type", i));
                    valueRect = EditorGUI.PrefixLabel(lineRect, new GUIContent(string.Format("     • {0}", objectNameProperty.stringValue)));

                    property.FindPropertyRelative(string.Format("objects.Array.data[{0}].id", i)).stringValue = objectNameProperty.stringValue;
                    SerializedProperty objectValueProperty = property.FindPropertyRelative(string.Format("objects.Array.data[{0}].value", i));
                    objectValueProperty.objectReferenceValue = EditorGUI.ObjectField(valueRect, objectValueProperty.objectReferenceValue, GameEventData.TypeFromEnum((GameEventData.SupportedObjectType)objectTypeProperty.enumValueIndex), true);

                    if (objectValueProperty.objectReferenceValue != null)
                    {
                        if (objectValueProperty.objectReferenceValue is CustomValueData)
                        {
                            CustomValueData cvd = (CustomValueData)objectValueProperty.objectReferenceValue;
                            lineRect.y += 18;

                            if (cvd.type == CustomValueData.ValueType.Bool)
                            {
                                EditorGUI.LabelField(lineRect, " ", "     (Bool Type)", EditorStyles.boldLabel);
                            }
                            else if (cvd.type == CustomValueData.ValueType.Int)
                            {
                                EditorGUI.LabelField(lineRect, " ", "     (int Type)", EditorStyles.boldLabel);
                            }
                            else if (cvd.type == CustomValueData.ValueType.Float)
                            {
                                EditorGUI.LabelField(lineRect, " ", "     (Float Type)", EditorStyles.boldLabel);
                            }
                            else if (cvd.type == CustomValueData.ValueType.String)
                            {
                                EditorGUI.LabelField(lineRect, " ", "     (String Type)", EditorStyles.boldLabel);
                            }
                        }
                    }
                }

                //Bools
                int numBools = gameEventObject.FindProperty("eventBools").arraySize;
                property.FindPropertyRelative("bools").arraySize = numBools;

                if (numBools > 0)
                {
                    lineRect.y += 18;
                    EditorGUI.LabelField(lineRect, "    Bools", EditorStyles.boldLabel);
                }

                for (int i = 0; i < numBools; i++)
                {
                    lineRect.y += 18;

                    SerializedProperty boolNameProperty = gameEventObject.FindProperty(string.Format("eventBools.Array.data[{0}]", i));
                    valueRect = EditorGUI.PrefixLabel(lineRect, new GUIContent(string.Format("     • {0}", boolNameProperty.stringValue)));

                    property.FindPropertyRelative(string.Format("bools.Array.data[{0}].id", i)).stringValue = boolNameProperty.stringValue;
                    SerializedProperty boolValueProperty = property.FindPropertyRelative(string.Format("bools.Array.data[{0}].value", i));
                    EditorGUI.PropertyField(valueRect, boolValueProperty, new GUIContent());
                }

                //Ints
                int numInts = gameEventObject.FindProperty("eventInts").arraySize;
                property.FindPropertyRelative("ints").arraySize = numInts;

                if (numInts > 0)
                {
                    lineRect.y += 18;
                    EditorGUI.LabelField(lineRect, "    Ints", EditorStyles.boldLabel);
                }

                for (int i = 0; i < numInts; i++)
                {
                    lineRect.y += 18;

                    SerializedProperty intNameProperty = gameEventObject.FindProperty(string.Format("eventInts.Array.data[{0}]", i));
                    valueRect = EditorGUI.PrefixLabel(lineRect, new GUIContent(string.Format("     • {0}", intNameProperty.stringValue)));

                    property.FindPropertyRelative(string.Format("ints.Array.data[{0}].id", i)).stringValue = intNameProperty.stringValue;
                    SerializedProperty intValueProperty = property.FindPropertyRelative(string.Format("ints.Array.data[{0}].value", i));
                    EditorGUI.PropertyField(valueRect, intValueProperty, new GUIContent());
                }

                //Floats
                int numFloats = gameEventObject.FindProperty("eventFloats").arraySize;
                property.FindPropertyRelative("floats").arraySize = numFloats;

                if (numFloats > 0)
                {
                    lineRect.y += 18;
                    EditorGUI.LabelField(lineRect, "    Floats", EditorStyles.boldLabel);
                }

                for (int i = 0; i < numFloats; i++)
                {
                    lineRect.y += 18;

                    SerializedProperty floatNameProperty = gameEventObject.FindProperty(string.Format("eventFloats.Array.data[{0}]", i));
                    valueRect = EditorGUI.PrefixLabel(lineRect, new GUIContent(string.Format("     • {0}", floatNameProperty.stringValue)));

                    property.FindPropertyRelative(string.Format("floats.Array.data[{0}].id", i)).stringValue = floatNameProperty.stringValue;
                    SerializedProperty floatValueProperty = property.FindPropertyRelative(string.Format("floats.Array.data[{0}].value", i));
                    EditorGUI.PropertyField(valueRect, floatValueProperty, new GUIContent());
                }

                //Strings
                int numStrings = gameEventObject.FindProperty("eventStrings").arraySize;
                property.FindPropertyRelative("strings").arraySize = numStrings;

                if (numStrings > 0)
                {
                    lineRect.y += 18;
                    EditorGUI.LabelField(lineRect, "    Strings", EditorStyles.boldLabel);
                }

                for (int i = 0; i < numStrings; i++)
                {
                    lineRect.y += 18;

                    SerializedProperty stringNameProperty = gameEventObject.FindProperty(string.Format("eventStrings.Array.data[{0}]", i));
                    valueRect = EditorGUI.PrefixLabel(lineRect, new GUIContent(string.Format("     • {0}", stringNameProperty.stringValue)));

                    property.FindPropertyRelative(string.Format("strings.Array.data[{0}].id", i)).stringValue = stringNameProperty.stringValue;
                    SerializedProperty stringValueProperty = property.FindPropertyRelative(string.Format("strings.Array.data[{0}].value", i));
                    EditorGUI.PropertyField(valueRect, stringValueProperty, new GUIContent());
                }
            }

            property.serializedObject.ApplyModifiedProperties();
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 18;

            if (property.FindPropertyRelative("gameEventData").objectReferenceValue != null)
            {
                SerializedObject gameEventObject = new SerializedObject(property.FindPropertyRelative("gameEventData").objectReferenceValue);

                int numBools = gameEventObject.FindProperty("eventBools").arraySize;
                int numInts = gameEventObject.FindProperty("eventInts").arraySize;
                int numFloats = gameEventObject.FindProperty("eventFloats").arraySize;
                int numStrings = gameEventObject.FindProperty("eventStrings").arraySize;
                int numObjects = gameEventObject.FindProperty("eventObjects").arraySize;
                int numTitles = 0;
                int delayHeight = 18 * 2;
                int extraHeight = 10;

                if (numBools > 0)
                {
                    numTitles++;
                }

                if (numInts > 0)
                {
                    numTitles++;
                }

                if (numFloats > 0)
                {
                    numTitles++;
                }

                if (numStrings > 0)
                {
                    numTitles++;
                }

                if (numObjects > 0)
                {
                    numTitles++;
                }

                height += (numBools + numInts + numFloats + numStrings + numObjects + numTitles) * 18 + delayHeight + extraHeight;

                for (int i = 0; i < numObjects; i++)
                {
                    SerializedProperty objectValueProperty = property.FindPropertyRelative(string.Format("objects.Array.data[{0}].value", i));

                    if (objectValueProperty.objectReferenceValue != null)
                    {
                        if (objectValueProperty.objectReferenceValue is CustomValueData)
                        {
                            height += 18;
                        }
                    }
                }
            }

            return height;
        }
    }
}