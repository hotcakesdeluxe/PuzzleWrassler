using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ArrayData", menuName = "TestArrayData")]
public class ArrayDataTest : ScriptableObject
{
	public ArrayLayout data  = new ArrayLayout(4);
}
