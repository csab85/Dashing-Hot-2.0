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
    }

    void SetWalkingAnimation()
    {
        _animator.SetInteger("Movement", 1);
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
    }

    #endregion
}
