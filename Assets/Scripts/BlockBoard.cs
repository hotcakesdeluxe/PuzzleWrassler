using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlockBoard : MonoBehaviour
{
    public int width = 8;
    public int height = 12;
    public Transform[,] blockGrid;
    void Awake()
    {
        blockGrid = new Transform[width, height];
    }

    public bool WithinBorders(Vector3 target)
    {
        return target.x > -1 &&
            target.x < width &&
            target.y > -1 &&
            target.y < height + 1;
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
        Debug.Log(col + " ," + row + " is empty?");
        if (WithinBorders(new Vector3(col, row, 0)))
        {
            return blockGrid[col, row] == null;
        }
        return false;
    }

    public bool ColorMatches(int col, int row, Transform block)
    {
        if (WithinBorders(new Vector3(col, row, 0)))
        {
            return blockGrid[col, row].GetComponent<Block>().colorIdx == block.GetComponent<Block>().colorIdx;
        }
        return false;
    }

    public void Clear(float col, float row)
    {
        blockGrid[(int)col, (int)row] = null;
    }

    public void Add(float col, float row, Transform obj)
    {
        blockGrid[(int)col, (int)row] = obj;
    }

    public void Delete(Transform block)
    {
        Vector2 pos = new Vector2(Mathf.Round(block.position.x), Mathf.Round(block.position.y));
        blockGrid[(int)pos.x, (int)pos.y] = null;
        Destroy(block.gameObject);
    }

    public bool WhatToDelete()
    {
        List<Transform> groupToDelete = new List<Transform>();

        for (int row = 0; row < 12; row++)
        {
            for (int col = 0; col < 6; col++)
            {
                List<Transform> currentGroup = new List<Transform>();

                if (blockGrid[col, row] != null)
                {
                    Transform current = blockGrid[col, row];
                    if (groupToDelete.IndexOf(current) == -1)
                    {
                        AddNeighbors(current, currentGroup);
                    }
                }

                if (currentGroup.Count >= 4)
                {
                    foreach (Transform block in currentGroup)
                    {
                        groupToDelete.Add(block);
                        Debug.Log(block.position.x + " ," + block.position.y);
                    }
                }
            }
        }
        Debug.Log("what to delete");
        if (groupToDelete.Count != 0)
        {
            DeleteUnits(groupToDelete);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DropAllColumns()
    {
        for (int row = 0; row < 12; row++)
        {
            for (int col = 0; col < 6; col++)
            {
                if (blockGrid[col, row] != null)
                {
                    Transform block = blockGrid[col, row];
                    block.gameObject.GetComponent<Block>().DropToFloor();
                }
            }
        }
    }

    void AddNeighbors(Transform currentBlock, List<Transform> currentGroup)
    {
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };
        if (currentGroup.IndexOf(currentBlock) == -1)
        {
            currentGroup.Add(currentBlock);
        }
        else
        {
            return;
        }

        foreach (Vector3 direction in directions)
        {
            int nextX = (int)(Mathf.Round(currentBlock.position.x) + Mathf.Round(direction.x));
            int nextY = (int)(Mathf.Round(currentBlock.position.y) + Mathf.Round(direction.y));

            if (!IsEmpty(nextX, nextY) && ColorMatches(nextX, nextY, currentBlock))
            {
                Transform nextBlock = blockGrid[nextX, nextY];
                AddNeighbors(nextBlock, currentGroup);
            }
        }
    }
    void DeleteUnits(List<Transform> blocksToDelete)
    {
        foreach (Transform block in blocksToDelete)
        {
            Delete(block);
        }
    }

    public bool AnyFallingBlocks()
    {
        for (int row = 11; row >= 0; row--)
        {
            for (int col = 0; col < 6; col++)
            {
                if (blockGrid[col, row] != null)
                {
                    if (blockGrid[col, row].gameObject.GetComponent<Block>().forcedDownwards)
                    {
                        return true;
                    }
                    else if (blockGrid[col, row].gameObject.GetComponent<Block>().activelyFalling)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
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
    public void DebugBoard()
    {
        Text text = GameObject.Find("Text").GetComponent<Text>();
        string boardContents = "";

        for (int row = height - 1; row >= 0; row--)
        {
            boardContents += $"{row} :";
            for (int col = 0; col < width; col++)
            {
                if (blockGrid[col, row] == null)
                {
                    boardContents += "o ";
                }
                else
                {
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
