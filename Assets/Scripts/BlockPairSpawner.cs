using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPairSpawner : MonoBehaviour
{
    [SerializeField] private BlockBoard _blockBoard;
    [SerializeField] private BlockPair _blockPairPrefab;
    [SerializeField] private Block _blockPrefab;
    private string _previewLeftType;
    private string _previewRightType;
    public BlockPair activeBlockPair { get; private set; }
    Stack<Block> blockObjectPool = new Stack<Block>();
    // Start is called before the first frame update
    void Start()
    {
        InitializePreviewBlocks();
        SpawnBlockPair();
    }

    public void SpawnBlockPair()
    {
        if (_blockBoard.WhatToDelete())
        {
            StartCoroutine(DelayDelete());
        }
        StartCoroutine(DelaySpawnRoutine());
    }

    public void InitializePreviewBlocks()
    {
        _previewLeftType = GetRandomElement();
        _previewRightType = GetRandomElement();
        _blockBoard.leftPreviewBlock.material.SetColor("_Color", GetColorByType(_previewLeftType));
        _blockBoard.rightPreviewBlock.material.SetColor("_Color", GetColorByType(_previewRightType));
    }

    private Block GetFreshBlock(string type)
    {
        Block retObj;

        if (blockObjectPool.Count > 0)
        {
            retObj = blockObjectPool.Pop();
        }
        else
        {

            retObj = GameObject.Instantiate<Block>(_blockPrefab, transform.position, Quaternion.identity, activeBlockPair.transform);
        }

        retObj.gameObject.SetActive(true);
        retObj.enabled = true;
        retObj.blockType = type;
        return retObj;
    }

    private void ReturnBlockToPool(Block obj)
    {

        if (obj != null)
        {
            obj.enabled = false;
            obj.gameObject.SetActive(false);

            blockObjectPool.Push(obj);
        }
    }
    private bool GameIsOver()
    {
        return
            _blockBoard.blockGrid[(int)transform.position.x, (int)transform.position.y] != null ||
            _blockBoard.blockGrid[(int)transform.position.x + 1, (int)transform.position.y] != null;
    }

    private string GetRandomElement()
    {
        float randomVal = Random.value * 4;
        if (randomVal <= 1) return "earth";
        else if (randomVal <= 2) return "air";
        else if (randomVal <= 3) return "water";
        else return "fire";
    }
    private Color GetColorByType(string type)
    {
        if (type == "earth")
        {
            return Color.green;
        }
        else if (type == "air")
        {
            return Color.cyan;
        }
        else if (type == "water")
        {
            return Color.blue;
        }
        else
        {
            return Color.red;
        }
    }

    IEnumerator DelayDelete()
    {
        _blockBoard.DropAllColumns();
        yield return new WaitUntil(() => !_blockBoard.AnyFallingBlocks());
        if (_blockBoard.WhatToDelete())
        {
            StartCoroutine(DelayDelete());
        }

    }

    IEnumerator DelaySpawnRoutine()
    {
        yield return new WaitUntil(() => !_blockBoard.AnyFallingBlocks() && !_blockBoard.WhatToDelete());
        if (GameIsOver())
        {
            Debug.Log("Game Over");
        }
        else
        {
            //spawn
            activeBlockPair = GameObject.Instantiate<BlockPair>(_blockPairPrefab, transform.position, Quaternion.identity, transform.parent);
            activeBlockPair.Initialize(_blockBoard, GetFreshBlock(_previewLeftType), GetFreshBlock(_previewRightType));
            activeBlockPair.spawnNextEvent.AddListener(SpawnBlockPair);
            InitializePreviewBlocks();
        }
    }
}
