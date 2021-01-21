using UnityEngine;
using System.Collections;


[System.Serializable]
public class ArrayLayout
{

    [System.Serializable]
    public struct rowData
    {
        public BlockColor[] row;
    }
	public int size;
    public rowData[] rows; //Grid of 7x7
	public ArrayLayout(int i) { rows = new rowData[i]; }
}
