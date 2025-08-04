using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : CharacterAnimation
{
    #region VARIABLES

    //COMPONENTS
    Animator _animator;
    StateMachine _stateMachine;
    PlayerStats _playerStats;

    //STATES

    #endregion

    #region METHODS

    void SetPunchAnimation()
    {
        _animator.Play("Punch");
    }

    void SetDashAnimation()
    {
        _animator.Play("Dash");
    }

    #endregion

    #region UNITY LIFECYCLE

    private void Start()
    {
        CharacterAnimationStart();

        //get components
        _animator = GetComponent<Animator>();
        _stateMachine = transform.parent.GetComponent<StateMachine>();
        _playerStats = transform.parent.GetComponent<PlayerStats>();

        //sign in to events
        _playerStats.OnHitPunch += SetPunchAnimation;
        _playerStats.OnDash += SetDashAnimation;
    }

    #endregion
}
