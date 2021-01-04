using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public static class ExtraMath
    {
        public static float Map(float x, float inMin, float inMax, float outMin, float outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        public static float MapClamped(float x, float inMin, float inMax, float outMin, float outMax)
        {
            if (outMax > outMin)
            {
                return Mathf.Clamp(Map(x, inMin, inMax, outMin, outMax), outMin, outMax);
            }
            else
            {
                return Mathf.Clamp(Map(x, inMin, inMax, outMin, outMax), outMax, outMin);
            }
        }

        public static float Manhattan3D(Vector3 pos1, Vector3 pos2)
        {
            return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y) + Mathf.Abs(pos1.z - pos2.z);
        }

        public static float Manhattan2D(Vector2 pos1, Vector2 pos2)
        {
            return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
        }

        public static float Manhattan2DFrom3D(Vector3 pos1, Vector3 pos2)
        {
            return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.z - pos2.z);
        }

        public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 up)
        {
            return Mathf.Atan2(Vector3.Dot(up, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }

        public static bool RandomBool()
        {
            return Random.value < 0.5f;
        }

        public static Vector3 RotatePointAroundAxis(Vector3 point, float angle, Vector3 axis)
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis);
            return q * point;
        }
        
        //Works for negative numbers too!
        public static int Modulus(int a, int b)
        {
            return (a % b + b) % b;
        }

        //Lists
        public static void Shuffle<T>(this IList<T> list)
        {
            int count = list.Count;

            for(int i = 0; i < count * 3; i++)
            {
                int random1 = Random.Range(0, count);
                int random2 = Random.Range(0, count);

                T temp = list[random1];
                list[random1] = list[random2];
                list[random2] = temp;
            }
        }

        //Quaternions
        public static Quaternion SmoothDampQuaternion(Quaternion rot, Quaternion target, ref Quaternion deriv, float time, float maxSpeed, float deltaTime)
        {
            // account for double-cover
            float Dot = Quaternion.Dot(rot, target);
            float Multi = Dot > 0f ? 1f : -1f;
            target.x *= Multi;
            target.y *= Multi;
            target.z *= Multi;
            target.w *= Multi;
            // smooth damp (nlerp approx)
            Vector4 Result = new Vector4(
                Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time, maxSpeed, deltaTime),
                Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time, maxSpeed, deltaTime),
                Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time, maxSpeed, deltaTime),
                Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time, maxSpeed, deltaTime)
            ).normalized;
            // compute deriv
            float dtInv = 1f / deltaTime;
            deriv.x = (Result.x - rot.x) * dtInv;
            deriv.y = (Result.y - rot.y) * dtInv;
            deriv.z = (Result.z - rot.z) * dtInv;
            deriv.w = (Result.w - rot.w) * dtInv;
            return new Quaternion(Result.x, Result.y, Result.z, Result.w);
        }
    }
}