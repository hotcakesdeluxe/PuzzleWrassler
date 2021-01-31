using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncerSounds : MonoBehaviour
{
    [SerializeField] private BlockBoard _blockBoard;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _clips;

    void Start()
    {
        _blockBoard.scoreUpdateEvent.AddListener(PlayAnnouncerClip);
    }
    private void PlayAnnouncerClip(int blocksDestroyed)
    {
        int randomClip = Random.Range(0, _clips.Length);
        _source.PlayOneShot(_clips[randomClip]);
    }
}
