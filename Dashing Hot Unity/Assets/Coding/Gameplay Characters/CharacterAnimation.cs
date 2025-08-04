using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAnimation : MonoBehaviour
{
    #region VARIABLES

    //COMPONENTS
    Animator _animator;
    StateMachine _stateMachine;
    CharacterStats _characterStats;

    //STATES

    #endregion

    #region METHODS

    //MOVEMENT 0 = IDLE; 1 = WALKING; 2 = DASHING; -1 = STUNNED;

    void SetIdleAnimation()
    {
        _animator.SetInteger("Movement", 0);
        _animator.SetBool("Propelled", false);
    }

    void SetWalkingAnimation()
    {
        _animator.SetInteger("Movement", 1);
    }

    void SetPropelledAnimation()
    {
        _animator.Play("Propelled");
        _animator.SetBool("Propelled", true);
    }

    void EnableAnimator()
    {
        _animator.enabled = true;
    }

    void DisableAnimator()
    {
        _animator.enabled = false;
    }

    protected void CharacterAnimationStart()
    {
        //get components
        _animator = GetComponent<Animator>();
        _stateMachine = transform.parent.GetComponent<StateMachine>();
        _characterStats = transform.parent.GetComponent<CharacterStats>();

        //sign in to events
        _characterStats.OnIdle += SetIdleAnimation;
        _characterStats.OnWalk += SetWalkingAnimation;
        _characterStats.OnPropelled += SetPropelledAnimation;
        _characterStats.OnEnableRagdoll += DisableAnimator;
        _characterStats.OnDisableRagdoll += EnableAnimator;
    }

    #endregion
}
