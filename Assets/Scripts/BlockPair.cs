using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PHL.Common.Utility;

public class BlockPair : MonoBehaviour
{
    public bool isFalling = true;
    public float fallSpeed = 0.5f;
    public float fallSpeedMultiplier = 1f;
    private BlockBoard _blockBoard;
    public Block activeLeftBlock;
    public Block activeRightBlock;
    public SecureEvent spawnNextEvent { get; private set; } = new SecureEvent();
    public void Initialize(BlockBoard blockboard, Block leftBlock, Block rightBlock)
    {
        _blockBoard = blockboard;
        activeLeftBlock = leftBlock;
        activeRightBlock = rightBlock;
        activeLeftBlock.Initialize(_blockBoard);
        activeRightBlock.Initialize(_blockBoard);
        rightBlock.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        UpdateGameBoard();
    }
    void Update()
    {
        //_blockBoard.DebugBoard();
        if (isFalling)
        {
            float actualFallSpeed = Mathf.Min(30f, fallSpeed * fallSpeedMultiplier);
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                actualFallSpeed = Mathf.Max(10f, actualFallSpeed);
            }
            if (ValidMove(Vector3.down))
            {
                ClearCurrentGameboardPosition();
                transform.position += Vector3.down * actualFallSpeed * Time.deltaTime;
                UpdateGameBoard();
            }
            else
            {
                BlockPairLanded();
            }
        }

    }

    public void TryHorizontalMove(Vector3 direction)
    {
        if (ValidMove(direction))
        {
            ClearCurrentGameboardPosition();
            transform.position += direction;
            UpdateGameBoard();
        }
    }
    public void TryRotate(int direction)
    {
        Vector3 rotateVector;
        if (direction > 0)
        {
            rotateVector = GetCounterClockwiseRotationVector(activeRightBlock.transform);
        }
        else
        {
            rotateVector = GetClockwiseRotationVector(activeRightBlock.transform);
        }

        if (ValidRotate(rotateVector))
        {
            ClearCurrentGameboardPosition();
            activeRightBlock.transform.position += rotateVector;
            UpdateGameBoard();
        }

    }
    Vector3 GetClockwiseRotationVector(Transform block)
    {
        Vector3 blockPos = RoundVector(block.position);

        if (Vector3.Distance(blockPos + Vector3.left, transform.position) < 1)
        {
            return new Vector3(-1, -1);
        }
        else if (Vector3.Distance(blockPos + Vector3.up, transform.position) < 1)
        {
            return new Vector3(-1, +1);
        }
        else if (Vector3.Distance(blockPos + Vector3.right, transform.position) < 1)
        {
            return new Vector3(+1, +1);
        }
        else if (Vector3.Distance(blockPos + Vector3.down, transform.position) < 1)
        {
            return new Vector3(+1, -1);
        }

        return new Vector3(0, 0);
    }

    Vector3 GetCounterClockwiseRotationVector(Transform block)
    {
        Vector3 blockPos = RoundVector(block.position);

        if (Vector3.Distance(blockPos + Vector3.left, transform.position) < 1)
        {
            return new Vector3(-1, +1);
        }
        else if (Vector3.Distance(blockPos + Vector3.up, transform.position) < 1)
        {
            return new Vector3(+1, +1);
        }
        else if (Vector3.Distance(blockPos + Vector3.right, transform.position) < 1)
        {
            return new Vector3(+1, -1);
        }
        else if (Vector3.Distance(blockPos + Vector3.down, transform.position) < 1)
        {
            return new Vector3(-1, -1);
        }

        return new Vector3(0, 0);
    }

    bool ValidMove(Vector3 direction)
    {
        foreach (Transform block in transform)
        {
            Vector3 newPosition = new Vector3(block.position.x + direction.x, Mathf.FloorToInt(block.position.y) + direction.y, 0);
            /*if (block.position.y + direction.y <= 0)
            {
                return false;
            }*/
            if (!_blockBoard.FreeSpace(newPosition, transform))
            {
                return false;
            }
        }
        return true;
    }

    bool ValidRotate(Vector3 direction)
    {
        Vector3 blockpos = activeRightBlock.transform.position;
        Vector3 newPosition = new Vector3(blockpos.x + direction.x, blockpos.y + direction.y);
        return _blockBoard.FreeSpace(newPosition, transform);
    }

    bool ActivelyFalling()
    {
        return activeLeftBlock.GetComponent<Block>().activelyFalling || activeRightBlock.GetComponent<Block>().activelyFalling;
    }

    private void DropBlocks()
    {
        foreach (Transform block in transform)
        {
            block.gameObject.GetComponent<Block>().DropToFloor();
        }
    }

    private void BlockPairLanded()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x),Mathf.FloorToInt(transform.position.y), transform.position.z);
        isFalling = false;
        DropBlocks();
        StartCoroutine(SpawnNextBlock());
    }

    void ClearCurrentGameboardPosition()
    {
        foreach (Transform block in transform)
        {
            _blockBoard.Clear(block.transform.position.x, block.transform.position.y);
        }
    }

    void UpdateGameBoard()
    {
        foreach (Transform block in transform)
        {
            Debug.Log(block.localPosition.x + " ," + block.localPosition.y + " block pos");
            _blockBoard.Add(block.position.x, block.position.y, block);
        }
    }

    IEnumerator SpawnNextBlock()
    {
        yield return new WaitUntil(() => !ActivelyFalling());
        spawnNextEvent.Invoke();
        //GameObject.Find("PuyoSpawner").GetComponent<PuyoSpawner>().SpawnPuyo();
    }

    public Vector3 RoundVector(Vector3 vect)
    {
        return new Vector2(Mathf.Round(vect.x), Mathf.Round(vect.y));
    }


}
