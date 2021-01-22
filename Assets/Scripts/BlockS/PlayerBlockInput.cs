﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using PHL.Common.Utility;
public class PlayerBlockInput : MonoBehaviour
{
    public bool isSecondPlayer = false;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private BlockPairSpawner _blockPairSpawner;
    private BlockPair _currentBlockPair;
    private string currentControlScheme;
    public SecureEvent moveEvent = new SecureEvent();
    public SecureEvent rotateEvent = new SecureEvent();
    private bool _gameIsOver = false;

    void Awake()
    {
        currentControlScheme = _playerInput.currentControlScheme;
        if (isSecondPlayer)
        {
            InputUser.PerformPairingWithDevice(Keyboard.current, _playerInput.user);
            _playerInput.user.ActivateControlScheme("Player2Keyboard");
        }
        /*foreach (var device in _playerInput.devices)
        {
            Debug.Log(device);
        }*/
        _blockPairSpawner.gameEndEvent.AddListener(GameOver);
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
            moveEvent.Invoke();
        }

    }
    public void OnMoveRight(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _currentBlockPair.TryHorizontalMove(Vector3.right);
            moveEvent.Invoke();
        }
    }
    public void OnRotateLeft(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _currentBlockPair.TryRotate(1);
            rotateEvent.Invoke();
        }
    }
    public void OnRotateRight(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _currentBlockPair.TryRotate(-1);
            rotateEvent.Invoke();
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

    public void OnRestart(InputAction.CallbackContext value)
    {
        if (_gameIsOver)
        {
            if (value.started)
            {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }
        }
    }

    public void OnQuitToMenu(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
    public void OnControlsChanged()
    {
        if (_playerInput.currentControlScheme != currentControlScheme)
        {
            currentControlScheme = _playerInput.currentControlScheme;
            //playerVisualsBehaviour.UpdatePlayerVisuals();
        }
    }

    public void GameOver()
    {
        _gameIsOver = true;
    }
}
