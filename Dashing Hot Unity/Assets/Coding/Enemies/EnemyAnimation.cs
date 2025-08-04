using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : CharacterAnimation
{
    #region VARIABLES

    //COMPONENTS
    Animator _animator;
    StateMachine _stateMachine;
    EnemyStats _enemyStats;

    //STATES

    #endregion

    #region METHODS



    #endregion

    #region UNITY LIFECYCLE

    private void Start()
    {
        CharacterAnimationStart();

        //get components
        _animator = GetComponent<Animator>();
        _stateMachine = transform.parent.GetComponent<StateMachine>();
        _enemyStats = transform.parent.GetComponent<EnemyStats>();

        //sign in to events
    }

    #endregion
}
