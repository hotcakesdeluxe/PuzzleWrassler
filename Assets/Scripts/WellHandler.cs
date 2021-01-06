using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellHandler : MonoBehaviour
{
    public BlockPair currBlockPair {get; private set;}
    public Vector3 spawnPoint {get; private set;}
    [SerializeField] private BlockPair _blockPairPrefab;
    //public int[] currentColumnHeights;
    public List<float> currentColumnHeights = new List<float>();
    //pool of block pairs
    Stack<BlockPair> blockPairPool = new Stack<BlockPair>();
    //need something to preview the next to spawn
    private void Awake()
    {
        spawnPoint = new Vector3(transform.position.x, transform.position.y + BlockWell.height, 0);
        for(int i = 0; i < BlockWell.width; i++)
        {
            currentColumnHeights.Add(0);
            //Debug.Log(currentColumnHeights[i]);
        }
        SpawnNext();
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

    public void AddBlockToColumn(BlockObject leftblock, BlockObject rightblock)
    {   //add height to each block's column
        currentColumnHeights[leftblock.column] += 1;
        currentColumnHeights[rightblock.column] += 1;
        Debug.Log(currentColumnHeights[0]);
        ReturnBlockPairToPool(currBlockPair);
        SpawnNext();
        Debug.Log("asdfasdf");
    }
}
