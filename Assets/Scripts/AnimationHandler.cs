using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private BlockBoard _blockBoard;
    [SerializeField] private Animator _animator;

    void Start()
    {
        _blockBoard.scoreUpdateEvent.AddListener(PlayAttackAnimation);
    }

    void PlayAttackAnimation(int blocksDestroyed)
    {
        _animator.SetInteger("AttackInt", blocksDestroyed);
    }
}
