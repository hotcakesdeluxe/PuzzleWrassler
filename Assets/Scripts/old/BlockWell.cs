using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWell : MonoBehaviour
{
    public static int width = 8;
    public static int height = 12;
    public static GameObject[,] blockGrid;
    public static bool insideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < width &&
                (int)pos.y >= 0);
    }
    void Awake()
    {
        blockGrid = new GameObject[width, height];
    }

    void OnDrawGizmos()
    {
        for(int i = 1; i < height+2; i++)
        {
            Debug.DrawLine(new Vector3(this.transform.position.x - 0.5f, i-0.5f, 0), new Vector3((this.transform.position.x+width)-0.5f, i-0.5f, 0), Color.white);
        }
        for(int i = 0; i < width+1; i++)
        {
            Debug.DrawLine(new Vector3((this.transform.position.x+i)-0.5f, 0.5f, 0), new Vector3((this.transform.position.x+i)-0.5f, height+0.5f, 0), Color.white);
        }
        
    }
}
