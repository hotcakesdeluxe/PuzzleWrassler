using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class ColliderTools
    {
        public static Vector3 GetColliderCenter(Collider col)
        {
            return col.bounds.center;
        }
    }
}
