﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlockBoard : MonoBehaviour
{
    public int width = 8;
    public int height = 12;
    public static Transform[,] blockGrid;
    void Awake()
    {
        blockGrid = new Transform[width, height];
    }
    public bool WithinBorders(Vector3 target)
    {
        return target.x > -1 &&
            target.x < width &&
            target.y > -1 &&
            target.y < height+1;
    }
    public bool FreeSpace(Vector3 target, Transform parentTransform)
    {
        if (WithinBorders(target))
        {
            //Debug.Log(Mathf.FloorToInt(target.x)+ " ,"+ Mathf.FloorToInt(target.y));
            return blockGrid[Mathf.FloorToInt(target.x), Mathf.FloorToInt(target.y)] == null ||
                blockGrid[Mathf.FloorToInt(target.x), Mathf.FloorToInt(target.y)].parent == parentTransform;
        }
        return false;
    }
    public bool IsEmpty(int col, int row)
    {
        if (WithinBorders(new Vector3(col, row, 0)))
        {
            return blockGrid[col, row] == null;
        }
        return false;
    }
    public void Clear(float col, float row)
    {
        blockGrid[Mathf.FloorToInt(col), Mathf.FloorToInt(row)] = null;
    }

    public void Add(float col, float row, Transform obj)
    {
        blockGrid[Mathf.FloorToInt(col), Mathf.FloorToInt(row)] = obj;
    }

    void OnDrawGizmos()
    {
        for (int i = 1; i < height + 2; i++)
        {
            Debug.DrawLine(new Vector3(this.transform.position.x - 0.5f, i - 0.5f, 0), new Vector3((this.transform.position.x + width) - 0.5f, i - 0.5f, 0), Color.white);
        }
        for (int i = 0; i < width + 1; i++)
        {
            Debug.DrawLine(new Vector3((this.transform.position.x + i) - 0.5f, 0.5f, 0), new Vector3((this.transform.position.x + i) - 0.5f, height + 0.5f, 0), Color.white);
        }

    }
    public void DebugBoard(){
        Text text = GameObject.Find("Text").GetComponent<Text>();
        string boardContents = "";

        for(int row = height-1; row >= 0; row--){
            boardContents += $"{row} :";
            for(int col = 0; col < width; col++ ){                
                if(blockGrid[col, row] == null){
                    boardContents += "o ";
                } else {
                    int idx = blockGrid[col, row].gameObject.GetComponent<Block>().colorIdx;
                    string[] colorArray = { "G", "C", "B", "R" };
                    boardContents += $"{colorArray[idx]} ";
                }
            }            
            boardContents += "\n";
        }
        text.text = boardContents;
    }
}