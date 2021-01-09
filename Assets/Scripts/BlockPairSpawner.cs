using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPairSpawner : MonoBehaviour
{
    [SerializeField] private BlockBoard _blockboard;
    [SerializeField] private BlockPair _blockPairPrefab;
    [SerializeField] private Block _blockPrefab;
    public BlockPair activeBlockPair{get; private set;}
    Stack<Block> blockObjectPool = new Stack<Block>();
    // Start is called before the first frame update
    void Start()
    {
        SpawnBlockPair();
    }

    public void SpawnBlockPair()
    {
        activeBlockPair = GameObject.Instantiate<BlockPair>(_blockPairPrefab, transform.position, Quaternion.identity);
        activeBlockPair.Initialize(_blockboard, GetFreshBlock(), GetFreshBlock());
        activeBlockPair.spawnNextEvent.AddListener(SpawnBlockPair);
    }
    private Block GetFreshBlock()
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
}
