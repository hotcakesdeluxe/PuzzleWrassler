using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWell : MonoBehaviour
{
    public static int width = 8;
    public static int height = 12;
    public Transform[,] _blockGrid;
    public static bool insideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < width &&
                (int)pos.y >= 0);
    }
    void Awake()
    {
        _blockGrid = new Transform[width, height];
    }

    void OnDrawGizmos()
    {
        for(int i = 1; i < height+2; i++)
        {
            Debug.DrawLine(new Vector3(this.transform.position.x, i, 0), new Vector3(this.transform.position.x+width, i, 0), Color.white);
        }
        for(int i = 0; i < width+1; i++)
        {
            Debug.DrawLine(new Vector3(this.transform.position.x+i, 1, 0), new Vector3(this.transform.position.x+i, height+1, 0), Color.white);
        }
        
    }
}
