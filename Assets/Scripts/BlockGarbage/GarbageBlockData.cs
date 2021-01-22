using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GarbageBlockData", menuName = "GarbageBlockData")]
public class GarbageBlockData : ScriptableObject
{
	public ArrayLayout data  = new ArrayLayout(4);
}
