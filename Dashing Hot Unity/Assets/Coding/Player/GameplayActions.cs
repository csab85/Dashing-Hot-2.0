using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameplayActions : MonoBehaviour
{
    //GAMEPLAY LOGIC IS ONLY HANDLED HERE IF THERE'S NO ISTATE SCRIPT

    #region VARS

    //imports
    StateMachine _stateMachine;
    PlayerStats _playerStats;
    CharacterPhysics _characterPhysics;
    Transform _mainCameraTransform;

    //states
    IdleState.Player _idleState;
    WalkingState.Player _walkingState;

    #endregion


    #region METHODS

    /// <summary>
    /// Runs DashCoroutine if player is in combat mode and can use skills
    /// </summary>
    /// <param name="context"></param>
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && _playerStats.isOnCombatMode)
        {
            if (!_playerStats.IsPropelled && !_playerStats.isUsingSkill)
            {
                _characterPhysics.PushSelf(_mainCameraTransform.forward, _playerStats.DashForce, _playerStats.DashDuration, false, false);
                _playerStats.CallOnDash();
            }
        }
    }

    public void Walk(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            //try changing state
            _stateMachine.ChangeState(_walkingState);

            //try creating var with current state (only assigns value if current state is of WalkingState class) --> this is to check if state changed to/is walking
            var walkingState = _stateMachine.CurrentState as WalkingState.Player;

            walkingState?.SetInput(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            //try changing state
            _stateMachine.ChangeState(_idleState);
        }
    }

    #endregion


    #region UNITY LIFECYCLE

    private void Start()
    {
        //get dependencies
        _playerStats = GetComponent<PlayerStats>();
        _stateMachine = GetComponent<StateMachine>();
        _characterPhysics = GetComponent<CharacterPhysics>();
        _mainCameraTransform = Camera.main.transform;

        //construct states
        _idleState = new IdleState.Player(gameObject);
        _walkingState = new WalkingState.Player(gameObject);
    }

    #endregion
}
