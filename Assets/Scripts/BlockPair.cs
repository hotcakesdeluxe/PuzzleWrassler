using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPair : MonoBehaviour
{
    [SerializeField] private BlockObject _blockPrefab;
    private WellHandler _wellHandler;
    public BlockObject leftBlock;
    public BlockObject rightBlock;
    private int orientation = 0;  // Left is pivot, right is oriented 90 * orientation degrees
    Stack<BlockObject> blockObjectPool = new Stack<BlockObject>();

    public void Initialize(WellHandler wellhandler)
    {
        _wellHandler = wellhandler;
        leftBlock = GetFreshBlock();
        rightBlock = GetFreshBlock();
        leftBlock.Initialize((BlockWell.width/2)-1);
        rightBlock.Initialize(BlockWell.width/2);

    }
    private void Update()
    {
        leftBlock.UpdatePosition(_wellHandler.currentColumnHeights[leftBlock.column] + 1);
        rightBlock.UpdatePosition(_wellHandler.currentColumnHeights[leftBlock.column] + 1);


        Debug.Log(_wellHandler.currentColumnHeights[leftBlock.column] + " leftblock column height");
        Debug.Log(_wellHandler.currentColumnHeights[rightBlock.column] + " rightBlock column height");
    }

    private void FixedUpdate()
    {
        if (!leftBlock.isFalling)
        {
            Debug.Log("leftblock not falling");
            leftBlock.transform.SetParent(null);
            _wellHandler.AddBlockToColumn(leftBlock);
        }
        if (!rightBlock.isFalling)
        {
            Debug.Log("rightBlock not falling");
            rightBlock.transform.SetParent(null);
            _wellHandler.AddBlockToColumn(rightBlock);
        }
        if(!leftBlock.isFalling && !rightBlock.isFalling)
        {
            _wellHandler.CheckForDestroyableblock();
        }
    }

    private BlockObject GetFreshBlock()
    {
        BlockObject retObj;

        if (blockObjectPool.Count > 0)
        {
            retObj = blockObjectPool.Pop();
        }
        else
        {

            retObj = GameObject.Instantiate<BlockObject>(_blockPrefab, _wellHandler.spawnPoint, Quaternion.identity, transform);
        }

        retObj.gameObject.SetActive(true);
        retObj.enabled = true;

        return retObj;
    }

    private void ReturnBlockToPool(BlockObject obj)
    {

        if (obj != null)
        {
            obj.enabled = false;
            obj.gameObject.SetActive(false);

            blockObjectPool.Push(obj);
        }
    }

    public void TryHorizontalMove(int direction)
    {
        if (leftBlock.isFalling)
        {
            if (leftBlock != null)
            {
                int newLeft = leftBlock.column + direction;
                if (newLeft < 0 || newLeft >= BlockWell.width) return;
                float newLeftColumnHeight = _wellHandler.currentColumnHeights[newLeft];
                if (leftBlock.transform.position.y < newLeftColumnHeight + 1) return;
            }
            if (leftBlock != null)
            {
                leftBlock.column += direction;
                //leftBlock.transform.position = new Vector3(leftBlock.column, leftBlock.transform.position.y, 0);
            }

        }
        if (rightBlock.isFalling)
        {
            if (rightBlock != null)
            {
                int newRight = rightBlock.column + direction;
                if (newRight < 0 || newRight >= BlockWell.width) return;
                float newRightColumnHeight = _wellHandler.currentColumnHeights[newRight];
                if (rightBlock.transform.position.y < newRightColumnHeight) return;
            }

            if (rightBlock != null)
            {
                rightBlock.column += direction;
                //rightBlock.transform.position = new Vector3(rightBlock.column, rightBlock.transform.position.y, 0);
            }
        }
    }

    public void TryRotate(int direction)
    {
        if (leftBlock == null || rightBlock == null)
        {
            return;
        }
        if (rightBlock.isFalling)
        {
            int newDirection = (direction + 4 + orientation) % 4;
            Vector3 testPositionOffset = Vector3.zero;
            if (newDirection == 0) testPositionOffset = new Vector3(1, 0, 0);
            else if (newDirection == 1) testPositionOffset = new Vector3(0, 1, 0);
            else if (newDirection == 2) testPositionOffset = new Vector3(-1, 0, 0);
            else if (newDirection == 3) testPositionOffset = new Vector3(0, -1, 0);

            Vector3 testPosition = leftBlock.transform.localPosition + testPositionOffset;
            Vector3 movePosition = leftBlock.transform.position + testPositionOffset;
            if (testPosition.x < leftBlock.transform.position.x || testPosition.x >= BlockWell.width)
            {
                TrySwapRotate(direction);
                return;
            }
            if (testPosition.x < 0)
            {
                testPosition.x = 0;
            }
            float newTestColumnHeight = _wellHandler.currentColumnHeights[Mathf.FloorToInt(testPosition.x)];

            if (testPosition.y < newTestColumnHeight)
            {
                Debug.Log("here");
                TrySwapRotate(direction);
                return;
            }

            orientation = newDirection;
            rightBlock.column = Mathf.FloorToInt(testPosition.x);

            rightBlock.transform.position = movePosition;
        }
    }

    public void TrySwapRotate(int direction)
    {
        if (leftBlock == null || rightBlock == null) return;
        if (rightBlock.isFalling)
        {
            int newDirection = (-direction + 4 + orientation) % 4;
            Vector3 testPositionOffset = Vector3.zero;
            if (newDirection == 0) testPositionOffset = new Vector3(1, 0, 0);
            else if (newDirection == 1) testPositionOffset = new Vector3(0, 1, 0);
            else if (newDirection == 2) testPositionOffset = new Vector3(-1, 0, 0);
            else if (newDirection == 3) testPositionOffset = new Vector3(0, -1, 0);

            Vector3 testPosition = leftBlock.transform.localPosition + testPositionOffset;
            Vector3 movePosition = leftBlock.transform.position + testPositionOffset;
            if (testPosition.x < leftBlock.transform.position.x || testPosition.x >= BlockWell.width)
            {

                DoSwap();
                return;
            }
            if (testPosition.x < 0)
            {
                testPosition.x = 0;
            }
            float newTestColumnHeight = _wellHandler.currentColumnHeights[Mathf.FloorToInt(testPosition.x)];
            if (testPosition.y < newTestColumnHeight)
            {
                DoSwap();
                Debug.Log("here do swap");
                return;
            }

            orientation = (newDirection + 2) % 4;
            rightBlock.column = leftBlock.column;
            rightBlock.transform.position = leftBlock.transform.position;
            leftBlock.column = Mathf.FloorToInt(testPosition.x);
            leftBlock.transform.position = movePosition;
        }
    }
    void DoSwap()
    {
        orientation = (orientation + 2) % 4;
        int tempColumn = rightBlock.column;
        Vector3 tempPosition = rightBlock.transform.position;
        rightBlock.column = leftBlock.column;
        rightBlock.transform.position = leftBlock.transform.position;
        leftBlock.column = tempColumn;
        leftBlock.transform.position = tempPosition;
    }
}