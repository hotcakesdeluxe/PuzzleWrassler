using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXTriggers : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    //[SerializeField] private BlockBoard _blockBoard;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _bellClip;

    void Start()
    {
        _gameState.gameStartEvent.AddListener(PlayStartingBell);
        _gameState.pinFallEvent.AddListener(EndBell);
    }

    private void PlayStartingBell()
    {
        _source.PlayOneShot(_bellClip);
    }
    private void EndBell()
    {
        StartCoroutine(EndBellRoutine());
    }
    IEnumerator EndBellRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        PlayStartingBell();
    }
}
