﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PHL.Common.Utility;
public class BlockPairSpawner : MonoBehaviour
{
    [SerializeField] private BlockBoard _blockBoard;
    [SerializeField] private BlockPair _blockPairPrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private Block _breakPrefab;
    [SerializeField] private GameObject[] _leftPreviewBlocks; //these arrays are just the two kinds of blocks, cube/sphere
    [SerializeField] private GameObject[] _rightPreviewBlocks;
    [SerializeField] private BlockGarbageData _opponentGarbage;
    [SerializeField] private ArrayDataTest _testGarbage;
    public int maxBreakBlockChance = 8;
    private int numberOfPairsSpawned = 0;
    private float _blockPairFallSpeed = 0.5f;
    private string _previewLeftType;
    private string _previewRightType;
    private bool _isLeftBreakerBlock;
    private bool _isRightBreakerBlock;
    public BlockPair activeBlockPair { get; private set; }
    public SecureEvent spawnPairEvent = new SecureEvent();
    Stack<Block> blockObjectPool = new Stack<Block>();
    // Start is called before the first frame update
    void Start()
    {
        InitializePreviewBlocks();
        SpawnBlockPair();
        //_blockBoard.deleteBlockEvent.AddListener(ReturnBlockToPool);
        for(int i = 0; i < _testGarbage.data.rows.Length; i++)
        {
            for(int j = 0; j < _testGarbage.data.rows[i].row.Length; j++)
            {
                Debug.Log(_testGarbage.data.rows[i].row[j]);
            }
        }
    }

    public void SpawnBlockPair()
    {
        if (_blockBoard.WhatToDelete())
        {
            StartCoroutine(DelayDelete());
        }
        StartCoroutine(DelaySpawnRoutine());
        numberOfPairsSpawned += 1;
    }
    
    [ContextMenu("spawn garbo")]
    public void SpawnGarbage( )
    {
        StartCoroutine(DelaySpawnGarbage());
    }

    public void InitializePreviewBlocks()
    {
        _previewLeftType = GetRandomElement();
        _previewRightType = GetRandomElement();
        //_leftPreviewBlock.material.SetColor("_Color", GetColorByType(_previewLeftType));
        //_leftPreviewBlock.rightPreviewBlock.material.SetColor("_Color", GetColorByType(_previewRightType));
        _isLeftBreakerBlock = IsBreakerBlock();
        _isRightBreakerBlock = IsBreakerBlock();
        if(_isLeftBreakerBlock)
        {
            _leftPreviewBlocks[0].SetActive(false);
            _leftPreviewBlocks[1].SetActive(true);
            _leftPreviewBlocks[1].GetComponent<MeshRenderer>().material.SetColor("_Color", GetColorByType(_previewLeftType));
        }
        else
        {
            _leftPreviewBlocks[0].SetActive(true);
            _leftPreviewBlocks[1].SetActive(false);
            _leftPreviewBlocks[0].GetComponent<MeshRenderer>().material.SetColor("_Color", GetColorByType(_previewLeftType));
        }
        if(_isRightBreakerBlock)
        {
            _rightPreviewBlocks[0].SetActive(false);
            _rightPreviewBlocks[1].SetActive(true);
            _rightPreviewBlocks[1].GetComponent<MeshRenderer>().material.SetColor("_Color", GetColorByType(_previewRightType));
        }
        else
        {
            _rightPreviewBlocks[0].SetActive(true);
            _rightPreviewBlocks[1].SetActive(false);
            _rightPreviewBlocks[0].GetComponent<MeshRenderer>().material.SetColor("_Color", GetColorByType(_previewRightType));
        }
    }

    private Block GetFreshBlock(string _type, bool _isBreakBlock)
    {
        Block retObj;

        if (blockObjectPool.Count > 0)
        {
            retObj = blockObjectPool.Pop();
        }
        else
        {
            if(!_isBreakBlock)
            {
                retObj = GameObject.Instantiate<Block>(_blockPrefab, transform.position, _blockPrefab.transform.rotation, activeBlockPair.transform);
            }
            else
            {
                retObj = GameObject.Instantiate<Block>(_breakPrefab, transform.position, _breakPrefab.transform.rotation, activeBlockPair.transform);
            }
        }

        retObj.gameObject.SetActive(true);
        retObj.enabled = true;
        retObj.blockType = _type;
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
    private bool IsBreakerBlock()
    {
        float randomVal = Random.value * 10;
        return (randomVal > maxBreakBlockChance);
    }

    private string GetRandomElement()
    {
        float randomVal = Random.value * 4;
        if (randomVal <= 1) return "strike";
        else if (randomVal <= 2) return "grapple";
        else if (randomVal <= 3) return "aerial";
        else return "submission";
    }
    private Color GetColorByType(string type)
    {
        if (type == "strike")
        {
            return Color.green;
        }
        else if (type == "grapple")
        {
            return Color.cyan;
        }
        else if (type == "aerial")
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
            activeBlockPair.Initialize(_blockBoard, GetFreshBlock(_previewLeftType, _isLeftBreakerBlock), GetFreshBlock(_previewRightType, _isRightBreakerBlock), _blockPairFallSpeed);
            if(numberOfPairsSpawned % 10 == 0)
            {
                _blockPairFallSpeed += 0.2f;
            }
            activeBlockPair.spawnNextEvent.AddListener(SpawnBlockPair);
            spawnPairEvent.Invoke();
            InitializePreviewBlocks();
        }
    }

    IEnumerator DelaySpawnGarbage()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x -= transform.position.x - 1;
        yield return new WaitUntil(() => !_blockBoard.AnyFallingBlocks() && !_blockBoard.WhatToDelete());
        foreach(var garbo in _opponentGarbage.firstRow)
        {
           Block currGarbo = GetFreshBlock(garbo.ToString(), false);
           currGarbo.Initialize(_blockBoard);
           currGarbo.transform.position = spawnPos;
           spawnPos.x += 1;
           currGarbo.DropToFloor();
        }
    }
}