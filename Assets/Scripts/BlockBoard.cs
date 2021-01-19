using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PHL.Common.Utility;
public class BlockBoard : MonoBehaviour
{
    public Wiggle wiggler;
    [SerializeField]private ParticleSystem _destroyParticle;
    private int _width = 8;
    private int _height = 12;
    private bool _containsBreaker;
    public Transform[,] blockGrid;
    public Text debugText;
    public SecureEvent<int> scoreUpdateEvent { get; private set; } = new SecureEvent<int>();
    public SecureEvent deleteBlockEvent { get; private set; } = new SecureEvent();
    void Awake()
    {
        _width += (int)transform.position.x;
        blockGrid = new Transform[_width, _height];
    }

    public bool WithinBorders(Vector3 target)
    {
        return target.x > (int)transform.position.x - 1 &&
            target.x < _width &&
            target.y > -1 &&
            target.y < _height;
    }

    public bool FreeSpace(Vector3 target, Transform parentTransform)
    {
        if (WithinBorders(target))
        {
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

    public bool ColorMatches(int col, int row, Transform block)
    {
        if (WithinBorders(new Vector3(col, row, 0)))
        {
            return blockGrid[col, row].GetComponent<Block>().colorIdx == block.GetComponent<Block>().colorIdx;
        }
        return false;
    }
    public bool BreakerMatches(int col, int row)
    {
        if (WithinBorders(new Vector3(col, row, 0)))
        {
            return blockGrid[col, row].tag == "Breaker";
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
        Instantiate(_destroyParticle, block.position, Quaternion.identity);
        Destroy(block.gameObject);
    }

    public bool WhatToDelete()
    {
        List<Transform> groupToDelete = new List<Transform>();
        for (int row = 0; row < _height - 1; row++)
        {
            for (int col = (int)transform.position.x; col < _width; col++)
            {
                List<Transform> currentGroup = new List<Transform>();

                if (blockGrid[col, row] != null)
                {
                    Transform current = blockGrid[col, row];
                    if (current.tag == "Breaker")
                    {
                        if (groupToDelete.IndexOf(current) == -1)
                        {
                            AddNeighbors(current, currentGroup);
                        }
                    }
                }

                if (currentGroup.Count >= 4)
                {
                    foreach (Transform block in currentGroup)
                    {
                        groupToDelete.Add(block);
                    }
                }
            }
        }
        if (groupToDelete.Count != 0)
        {
            scoreUpdateEvent.Invoke(groupToDelete.Count);
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
        for (int row = 0; row < _height - 1; row++)
        {
            for (int col = (int)transform.position.x; col < _width; col++)
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
        deleteBlockEvent.Invoke();
        StartCoroutine(CameraShake());
        foreach (Transform block in blocksToDelete)
        {
            Delete(block);
        }
    }

    public bool AnyFallingBlocks()
    {
        for (int row = _height - 1; row >= 0; row--)
        {
            for (int col = (int)transform.position.x; col < _width; col++)
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
    IEnumerator CameraShake()
    {
        wiggler.enabled = true;
        yield return new WaitForSeconds(1f);
        wiggler.enabled = false;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            for (int i = 1; i < _height + 2; i++)
            {
                Debug.DrawLine(new Vector3(this.transform.position.x - 0.5f, i - 0.5f, 0), new Vector3((this.transform.position.x + _width) - 0.5f, i - 0.5f, 0), Color.white);
            }
            for (int i = 0; i < _width + 1; i++)
            {
                Debug.DrawLine(new Vector3((this.transform.position.x + i) - 0.5f, 0.5f, 0), new Vector3((this.transform.position.x + i) - 0.5f, _height + 0.5f, 0), Color.white);
            }
        }
    }
    public void DebugBoard()
    {
        //Text text = GameObject.Find("Text").GetComponent<Text>();
        string boardContents = "";

        for (int row = _height - 1; row >= 0; row--)
        {
            boardContents += $"{row} :";
            for (int col = (int)transform.position.x; col < _width; col++)
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
        debugText.text = boardContents;
    }
}
