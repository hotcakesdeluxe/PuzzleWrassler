using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PHL.Common.Utility;
public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private BlockBoard _blockBoard;
    [SerializeField]private BlockPairSpawner _blockPairSpawner;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationHandler _opponentAnimationHandler;
    public bool isSecondPlayer;
    public SecureEvent<int> hitReactEvent {get; private set;} = new SecureEvent<int>();
    public SecureEvent pinEvent {get; private set;} = new SecureEvent();

    void Start()
    {
        _blockBoard.scoreUpdateEvent.AddListener(PlayAttackAnimation);
        _blockPairSpawner.gameEndEvent.AddListener(PlayPinned);
        _opponentAnimationHandler.hitReactEvent.AddListener(PlayHitReactAnimation);
        _opponentAnimationHandler.pinEvent.AddListener(PlayPin);
        _animator.SetBool("IsMirror", isSecondPlayer);
    }

    void PlayAttackAnimation(int blocksDestroyed)
    {
        //_animator.SetInteger("AttackInt", blocksDestroyed);
        if(blocksDestroyed == 4)
        {
            _animator.Play("SuperKick");
        }
        else if ( blocksDestroyed > 4 && blocksDestroyed < 8)
        {
            _animator.Play("FiremanSlam");
        }
        else if(blocksDestroyed > 8)
        {
            _animator.Play("Stunner");
        }
        hitReactEvent.Invoke(blocksDestroyed);
    }

    void PlayHitReactAnimation(int blocksDestroyed)
    {
        //_animator.SetInteger("SellInt", blocksDestroyed);
        if(blocksDestroyed == 4)
        {
            _animator.Play("SuperKickSell");
        }
        else if ( blocksDestroyed > 4 && blocksDestroyed < 8)
        {
            _animator.Play("FiremanSlamSell");
        }
        else if(blocksDestroyed > 8)
        {
            _animator.Play("StunnerSell");
        }
    }

    void PlayPinned()
    {
        _animator.Play("RollupSell");
        pinEvent.Invoke();
    }
    void PlayPin()
    {
        _animator.Play("Rollup");
    }
}
