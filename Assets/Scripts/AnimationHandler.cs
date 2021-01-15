using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PHL.Common.Utility;
public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private BlockBoard _blockBoard;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationHandler _opponentAnimationHandler;
    public SecureEvent<int> hitReactEvent {get; private set;} = new SecureEvent<int>();

    void Start()
    {
        _blockBoard.scoreUpdateEvent.AddListener(PlayAttackAnimation);
        _opponentAnimationHandler.hitReactEvent.AddListener(PlayHitReactAnimation);
    }

    void PlayAttackAnimation(int blocksDestroyed)
    {
        _animator.SetInteger("AttackInt", blocksDestroyed);
        hitReactEvent.Invoke(blocksDestroyed);
    }

    void PlayHitReactAnimation(int blocksDestroyed)
    {
        _animator.SetInteger("SellInt", blocksDestroyed);
    }
}
