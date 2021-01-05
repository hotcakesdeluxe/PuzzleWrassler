using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInput : MonoBehaviour
{
    public WellHandler wellHandler;
    private BlockPair _currrentBlockPair;
    private float leftHoldTime = 0;
    private float rightHoldTime = 0;
    private float holdTime = 0.5f;  // Hold time until blocks slide

    private void Update()
    {
        _currrentBlockPair = wellHandler.currBlockPair;
        CheckInput();
    }

    private void CheckInput()
    {
        //tap button
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _currrentBlockPair.TryHorizontalMove(-1);
            leftHoldTime = 0;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _currrentBlockPair.TryHorizontalMove(1);
            rightHoldTime = 0;
        }
        //held button
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            leftHoldTime += Time.deltaTime;
            if (leftHoldTime > holdTime)
            {
                _currrentBlockPair.TryHorizontalMove(-1);
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rightHoldTime += Time.deltaTime;
            if (rightHoldTime > holdTime)
            {
                _currrentBlockPair.TryHorizontalMove(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _currrentBlockPair.TryRotate(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _currrentBlockPair.TryRotate(-1);
        }
    }
}
