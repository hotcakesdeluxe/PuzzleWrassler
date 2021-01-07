using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellHandler : MonoBehaviour
{
    [SerializeField] private DestroyBlock _destroyBlock;
    public BlockPair currBlockPair { get; private set; }
    public Vector3 spawnPoint { get; private set; }
    [SerializeField] private BlockPair _blockPairPrefab;
    //public int[] currentColumnHeights;
    public List<int> currentColumnHeights = new List<int>();
    //pool of block pairs
    Stack<BlockPair> blockPairPool = new Stack<BlockPair>();
    private bool _gameStart = false;
    //need something to preview the next to spawn
    private void Awake()
    {
        _destroyBlock.spawnNextEvent.AddListener(SpawnNext);
        spawnPoint = new Vector3(transform.position.x, transform.position.y + BlockWell.height, 0);
        for (int i = 0; i < BlockWell.width; i++)
        {
            currentColumnHeights.Add(0);
            //Debug.Log(currentColumnHeights[i]);
        }
        if(!_gameStart)
        {
            Debug.Log("game start");
            _gameStart = true;
            SpawnNext();
        }
        
    }

    private void SpawnNext()
    {
        BlockPair newPair = GetFreshBlockPair();
        newPair.Initialize(this);
        currBlockPair = newPair;
    }

    private BlockPair GetFreshBlockPair()
    {
        BlockPair retObj;

        if (blockPairPool.Count > 0)
        {
            retObj = blockPairPool.Pop();
        }
        else
        {

            retObj = GameObject.Instantiate<BlockPair>(_blockPairPrefab, spawnPoint, Quaternion.identity, transform);
        }

        retObj.gameObject.SetActive(true);
        retObj.enabled = true;

        return retObj;
    }

    private void ReturnBlockPairToPool(BlockPair obj)
    {

        if (obj != null)
        {
            obj.enabled = false;
            obj.gameObject.SetActive(false);

            blockPairPool.Push(obj);
        }
    }

    public void AddBlockToColumn(BlockObject currentBlock)
    {   //add height to each block's column
        
        currentColumnHeights[currentBlock.column]++;
        Debug.Log(currentColumnHeights[currentBlock.column]+" current column height");
        if (currentColumnHeights[currentBlock.column] > BlockWell.height)
        {
            //game over
        }
        else
        {
            //leftblock.transform.position = new Vector3(leftblock.column, currentColumnHeights[leftblock.column], 0);
            if (BlockWell.blockGrid[currentBlock.column, currentColumnHeights[currentBlock.column] - 1] != null)
            {
                Debug.Log("WARNING: GRID SPOT OCCUPIED: " + currentBlock.column + ", " + currentColumnHeights[currentBlock.column]);
            }
            BlockWell.blockGrid[currentBlock.column, currentColumnHeights[currentBlock.column] - 1] = currentBlock.gameObject;
            
            
        }
    }
    public void CheckForDestroyableblock()
    {
        ReturnBlockPairToPool(currBlockPair);
        _destroyBlock.CheckForDestroyBlocks(1);
    }
}
