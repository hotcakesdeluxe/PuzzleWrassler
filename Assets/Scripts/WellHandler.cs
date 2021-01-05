using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellHandler : MonoBehaviour
{
    public BlockPair currBlockPair {get; private set;}
    public Vector3 spawnPoint {get; private set;}
    [SerializeField] private BlockPair _blockPairPrefab;
    public int[] currentColumnHeights;
    //pool of block pairs
    Stack<BlockPair> blockPairPool = new Stack<BlockPair>();
    //need something to preview the next to spawn
    private void Awake()
    {
        currentColumnHeights = new int[BlockWell.width];
        spawnPoint = new Vector3(this.transform.position.x, this.transform.position.y + BlockWell.height, 0);
        foreach (int num in currentColumnHeights)
        {
            Debug.Log(num);
        }
        SpawnNext();
    }

    private void SpawnNext()
    {
        BlockPair newPair = GetFreshNoteObject();
        newPair.Initialize(this);
        currBlockPair = newPair;
    }

    private BlockPair GetFreshNoteObject()
    {
        BlockPair retObj;

        if (blockPairPool.Count > 0)
        {
            retObj = blockPairPool.Pop();
        }
        else
        {
            
            retObj = GameObject.Instantiate<BlockPair>(_blockPairPrefab, spawnPoint, Quaternion.identity, this.transform);
        }

        retObj.gameObject.SetActive(true);
        retObj.enabled = true;

        return retObj;
    }

    // Deactivates and returns a Note Object to the pool.
    private void ReturnNoteObjectToPool(BlockPair obj)
    {

        if (obj != null)
        {
            obj.enabled = false;
            obj.gameObject.SetActive(false);

            blockPairPool.Push(obj);
        }
    }

    public void AddBlockToColumn()
    {
        currBlockPair.leftBlock.transform.position = new Vector3(currBlockPair.leftBlock.column, 0, 0);//change y to height of current column
        currBlockPair.rightBlock.transform.position = new Vector3(currBlockPair.rightBlock.column, 0, 0);//change y to height of current column
    }
}
