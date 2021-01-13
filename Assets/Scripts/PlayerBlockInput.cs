using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.Users;
public class PlayerBlockInput : MonoBehaviour
{
    public bool isSecondPlayer = false;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private BlockPairSpawner _blockPairSpawner;
    private BlockPair _currentBlockPair;
    private string currentControlScheme;
    
    void Awake()
    {
        currentControlScheme = _playerInput.currentControlScheme;
        if(isSecondPlayer)
        {
            InputUser.PerformPairingWithDevice(Keyboard.current, _playerInput.user);
            _playerInput.user.ActivateControlScheme("Player2Keyboard");
        }
        foreach(var device in _playerInput.devices)
        {
            Debug.Log(device);
        }
        
        //InputUser.PerformPairingWithDevice(Gamepad.current, _playerInput.user);

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
            if (value.interaction is HoldInteraction)
            {
                _currentBlockPair.isFastDropping = true;
            }
        }
        if (value.canceled)
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
