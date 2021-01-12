using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
public class PlayerBlockInput : MonoBehaviour
{
    public bool isSecondPlayer = false;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private BlockPairSpawner _blockPairSpawner;
    private BlockPair _currentBlockPair;
    private float leftHoldTime = 0;
    private float rightHoldTime = 0;
    private float holdTime = 0.5f;  // Hold time until blocks slide
    private string currentControlScheme;

    void Awake()
    {
        currentControlScheme = _playerInput.currentControlScheme;
    }

    // Update is called once per frame
    void Update()
    {
        _currentBlockPair = _blockPairSpawner.activeBlockPair;
    }
    public void OnMoveLeft(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _currentBlockPair.TryHorizontalMove(Vector3.left);
        }
    }
    public void OnMoveRight(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _currentBlockPair.TryHorizontalMove(Vector3.right);
            rightHoldTime = 0;
        }
    }
    public void OnRotateLeft(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _currentBlockPair.TryRotate(1);
        }
    }
    public void OnRotateRight(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _currentBlockPair.TryRotate(-1);
        }
    }
    public void OnDrop(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Debug.Log("here");
            if (value.interaction is HoldInteraction)
            {
                Debug.Log("here");
                _currentBlockPair.isFastDropping = true;
            }
        }
        if(value.canceled)
        {
            _currentBlockPair.isFastDropping = false;
        }     
    }
    public void OnControlsChanged()
    {
        Debug.Log("here");

        if (_playerInput.currentControlScheme != currentControlScheme)
        {
            currentControlScheme = _playerInput.currentControlScheme;
            //playerVisualsBehaviour.UpdatePlayerVisuals();
        }
    }
}
