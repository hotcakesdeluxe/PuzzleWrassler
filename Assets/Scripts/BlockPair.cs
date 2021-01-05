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

    public void Initialize(WellHandler wellhandler)
    {
        _wellHandler = wellhandler;
        leftBlock = GameObject.Instantiate<BlockObject>(_blockPrefab, new Vector3(this.transform.position.x, BlockWell.height + 1, 0), Quaternion.identity, this.transform);
        rightBlock = GameObject.Instantiate<BlockObject>(_blockPrefab, new Vector3(this.transform.position.x + 1, BlockWell.height + 1, 0), Quaternion.identity, this.transform);
        leftBlock.Initialize(0);
        rightBlock.Initialize(1);
    }
    private void Update()
    {
        leftBlock.UpdatePosition();
        rightBlock.UpdatePosition();
        Debug.Log(_wellHandler.currentColumnHeights[leftBlock.column]);
        if (leftBlock.transform.position.y <= _wellHandler.currentColumnHeights[leftBlock.column] + 1)
        {
            leftBlock.isFalling = false;
        }
        if (rightBlock.transform.position.y <= _wellHandler.currentColumnHeights[rightBlock.column] + 1)
        {
            rightBlock.isFalling = false;
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
                leftBlock.transform.position = new Vector3(leftBlock.column, leftBlock.transform.position.y, 0);
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
                rightBlock.transform.position = new Vector3(rightBlock.column, rightBlock.transform.position.y, 0);
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

            Vector3 testPosition = leftBlock.transform.position + testPositionOffset;
            if (testPosition.x < 0 || testPosition.x >= BlockWell.width)
            {
                TrySwapRotate(direction);
                return;
            }

            float newTestColumnHeight = _wellHandler.currentColumnHeights[Mathf.FloorToInt(testPosition.x)];
            if (testPosition.y < newTestColumnHeight)
            {
                TrySwapRotate(direction);
                return;
            }

            orientation = newDirection;
            rightBlock.column = Mathf.FloorToInt(testPosition.x);
            rightBlock.transform.position = testPosition;
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

            Vector3 testPosition = leftBlock.transform.position + testPositionOffset;
            if (testPosition.x < 0 || testPosition.x >= BlockWell.width)
            {
                DoSwap();
                return;
            }

            float newTestColumnHeight = _wellHandler.currentColumnHeights[Mathf.FloorToInt(testPosition.x)];
            if (testPosition.y < newTestColumnHeight)
            {
                DoSwap();
                return;
            }

            orientation = (newDirection + 2) % 4;
            rightBlock.column = leftBlock.column;
            rightBlock.transform.position = leftBlock.transform.position;
            leftBlock.column = Mathf.FloorToInt(testPosition.x);
            leftBlock.transform.position = testPosition;
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