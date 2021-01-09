using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInput : MonoBehaviour
{
    public BlockPairSpawner blockPairSpawner;
    private BlockPair _currentBlockPair;
    private float leftHoldTime = 0;
    private float rightHoldTime = 0;
    private float holdTime = 0.5f;  // Hold time until blocks slide

    private void Update()
    {
        _currentBlockPair = blockPairSpawner.activeBlockPair;
        CheckInput();
    }

    private void CheckInput()
    {
        //tap button
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _currentBlockPair.TryHorizontalMove(Vector3.left);
            leftHoldTime = 0;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _currentBlockPair.TryHorizontalMove(Vector3.right);
            rightHoldTime = 0;
        }
        //held button
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            leftHoldTime += Time.deltaTime;
            if (leftHoldTime > holdTime)
            {
                _currentBlockPair.TryHorizontalMove(Vector3.left);
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rightHoldTime += Time.deltaTime;
            if (rightHoldTime > holdTime)
            {
                _currentBlockPair.TryHorizontalMove(Vector3.right);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentBlockPair.TryRotate(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _currentBlockPair.TryRotate(-1);
        }
    }
}
