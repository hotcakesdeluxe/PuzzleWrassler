using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private PlayerBlockInput _input;
    [SerializeField] private BlockBoard _blockBoard;
    [SerializeField] private BlockPairSpawner _spawner;
    [SerializeField] private AudioClip _moveClip;
    [SerializeField] private AudioClip _rotateClip;
    [SerializeField] private AudioClip _landedClip;
    [SerializeField] private AudioClip _hitClip;

    void Start()
    {
        _input.moveEvent.AddListener(PlayMoveSound);
        _input.rotateEvent.AddListener(PlayRotateSound);
        _spawner.spawnPairEvent.AddListener(UpdateCurrentBlockPair);
        _blockBoard.deleteBlockEvent.AddListener(PlayDeleteBlockSound);
    }

    private void PlayRotateSound()
    {
        _source.PlayOneShot(_rotateClip);
    }
    private void PlayMoveSound()
    {
        _source.PlayOneShot(_moveClip);
    }
    private void UpdateCurrentBlockPair()
    {
        _spawner.activeBlockPair.landedEvent.AddListener(PlayLandedSound);
    }
    private void PlayLandedSound()
    {
        _source.PlayOneShot(_landedClip);
    }
    private void PlayDeleteBlockSound()
    {
        _source.PlayOneShot(_hitClip);
    }

}
