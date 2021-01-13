using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public static class ExtensionFunctions
    {
        //Layers
        public static void SetLayerRecursively(this GameObject gameObject, string layerName)
        {
            gameObject.SetLayerRecursively(LayerMask.NameToLayer(layerName));
        }

        public static void SetLayerRecursively(this GameObject gameObject, int newLayer)
        {
            gameObject.layer = newLayer;
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.layer = newLayer;
                if (child.childCount > 0)
                {
                    child.gameObject.SetLayerRecursively(newLayer);
                }
            }
        }
        
        //Floats
        public static float Abs(this float value)
        {
            return Mathf.Abs(value);
        }

        public static float Distance(this float value, float otherValue)
        {
            return Mathf.Abs(value - otherValue);
        }

        public static float RadToDeg(this float radians)
        {
            return radians * Mathf.Rad2Deg;
        }

        public static float DegToRad(this float degrees)
        {
            return degrees * Mathf.Deg2Rad;
        }

        //Vectors
        public static float[] ToFloatArray(this Vector2 vector2)
        {
            float[] floatArray = new float[2]
            {
                vector2.x,
                vector2.y
            };

            return floatArray;
        }

        public static float[] ToFloatArray(this Vector3 vector3)
        {
            float[] floatArray = new float[3]
            {
                vector3.x,
                vector3.y,
                vector3.z
            };

            return floatArray;
        }

        public static Vector3 ToVector3(this float[] floatArray)
        {
            Vector3 vector3 = new Vector3(floatArray[0], floatArray[1], floatArray[2]);
            return vector3;
        }

        public static Vector2 ToVector2(this float[] floatArray)
        {
            Vector3 vector2 = new Vector2(floatArray[0], floatArray[1]);
            return vector2;
        }

        public static Vector2 Parse(this Vector2 vector2, string serialized)
        {
            char[] charsToTrim = { '(', ')', ' ' };

            string[] pieces = serialized.Split(',');
            vector2.x = float.Parse(pieces[0].Trim(charsToTrim));
            vector2.y = float.Parse(pieces[1].Trim(charsToTrim));

            return vector2;
        }

        public static Vector3 Parse(this Vector3 vector3, string serialized)
        {
            char[] charsToTrim = { '(', ')', ' ' };

            string[] pieces = serialized.Split(',');
            vector3.x = float.Parse(pieces[0].Trim(charsToTrim));
            vector3.y = float.Parse(pieces[1].Trim(charsToTrim));
            vector3.z = float.Parse(pieces[2].Trim(charsToTrim));

            return vector3;
        }

        public static bool InRange(this Vector2 vector2, float value)
        {
            return value >= vector2.x && value <= vector2.y;
        }

        public static float RandomBetween(this Vector2 vector2)
        {
            return UnityEngine.Random.Range(vector2.x, vector2.y);
        }

        public static float Midpoint(this Vector2 vector2)
        {
            return (vector2.x + vector2.y) / 2f;
        }

        public static float Clamp(this Vector2 vector, float value)
        {
            return Mathf.Clamp(value, vector.x, vector.y);
        }

        public static Vector2 XY(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }

        public static Vector2 XZ(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }

        public static Vector2 YZ(this Vector3 vector3)
        {
            return new Vector2(vector3.y, vector3.z);
        }

        public static float AngleInDegrees(this Vector2 vector2)
        {
            Vector2 normalized = vector2.normalized;
            return Mathf.Atan2(normalized.x, normalized.y) * Mathf.Rad2Deg;
        }

        public static float AngleInRadians(this Vector2 vector2)
        {
            Vector2 normalized = vector2.normalized;
            return Mathf.Atan2(normalized.x, normalized.y);
        }

        public static Vector3 NearestPointOnLine(this Vector3 point, Vector3 start, Vector3 end)
        {
            Vector3 line = (end - start);
            float lineLength = line.magnitude;
            line = line / lineLength;

            Vector3 v = point - start;
            float d = Vector3.Dot(v, line);
            d = Mathf.Clamp(d, 0f, lineLength);
            return start + line * d;
        }

        public static float Lerp(this Vector2 vector2, float time)
        {
            return Mathf.Lerp(vector2.x, vector2.y, time);
        }

        public static float LerpUnclamped(this Vector2 vector2, float time)
        {
            return Mathf.LerpUnclamped(vector2.x, vector2.y, time);
        }

        public static float LerpAngle(this Vector2 vector2, float time)
        {
            return Mathf.LerpAngle(vector2.x, vector2.y, time);
        }

        public static float InverseLerp(this Vector2 vector2, float value)
        {
            return Mathf.InverseLerp(vector2.x, vector2.y, value);
        }

        public static Vector2 AngleToVector2Direction(this float degrees)
        {
            return new Vector2(Mathf.Sin(degrees * Mathf.Deg2Rad), Mathf.Cos(degrees * Mathf.Deg2Rad));
        }

        public static Vector3 AngleToVector3Direction(this float degrees)
        {
            return new Vector3(Mathf.Sin(degrees * Mathf.Deg2Rad), 0, Mathf.Cos(degrees * Mathf.Deg2Rad));
        }

        //Strings
        public static List<int> AllIndexOf(this string text, string str)
        {
            int index = text.IndexOf(str);

            if(index <= -1)
            {
                return null;
            }

            List<int> allIndexOf = new List<int>();

            while (index != -1)
            {
                allIndexOf.Add(index);
                index = text.IndexOf(str, index + str.Length);
            }

            return allIndexOf;
        }

        //Animators
        public static void SetLayerWeight(this Animator animator, string layerName, float weight)
        {
            animator.SetLayerWeight(animator.GetLayerIndex(layerName), weight);
        }

        public static float GetLayerWeight(this Animator animator, string layerName)
        {
            return animator.GetLayerWeight(animator.GetLayerIndex(layerName));
        }

        //Collision
        public static Collider[] OverlapBox(this BoxCollider boxCollider, int layerMask)
        {
            return Physics.OverlapBox(boxCollider.transform.position, boxCollider.size, boxCollider.transform.rotation, layerMask);
        }

        public static Collider[] OverlapBox(this BoxCollider boxCollider, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            return Physics.OverlapBox(boxCollider.transform.position, boxCollider.size * 0.5f, boxCollider.transform.rotation, layerMask, queryTriggerInteraction);
        }

        public static Collider[] OverlapSphere(this SphereCollider sphereCollider, int layerMask)
        {
            return Physics.OverlapSphere(sphereCollider.transform.position, sphereCollider.radius, layerMask);
        }

        public static Collider[] OverlapSphere(this SphereCollider sphereCollider, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            return Physics.OverlapSphere(sphereCollider.transform.position, sphereCollider.radius, layerMask, queryTriggerInteraction);
        }

        //Character Controllers
        public static void Teleport(this CharacterController characterController, Vector3 position)
        {
            characterController.enabled = false;
            characterController.transform.position = position;
            characterController.enabled = true;
        }

        //Arrays and Lists
        public static T Random<T>(this T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static T Random<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        //Colors
        public static Color ToColor(this Vector4 v4)
        {
            return new Color(v4.x, v4.y, v4.z, v4.w);
        }

        public static Color ToColor(this Vector3 v3)
        {
            return new Color(v3.x, v3.y, v3.z, 1);
        }

        public static Vector4 ToVector4(this Color c)
        {
            return new Vector4(c.r, c.g, c.b, c.a);
        }

        public static Vector3 ToVector3(this Color c)
        {
            return new Vector3(c.r, c.g, c.b);
        }
    }
}