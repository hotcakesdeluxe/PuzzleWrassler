using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Position", menuName = "PHL/Position", order = 0)]
public class PositionData : ScriptableObject
{
    public Vector3 position;
    public float angle;
}