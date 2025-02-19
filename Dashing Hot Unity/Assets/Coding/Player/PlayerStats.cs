using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : CharacterStats
{
    #region VARS

    //IMPORTS
    Transform richBodyTransform;

    //STATES
    public enum States
    {
        Idle,
        Walking
    }

    public States state;

    public bool combatMode;
    public bool usingSkill;

    #endregion


    #region METHODS

    public override void SetState<TState>(TState targetState)
    {
        //check if target state is of the states enum type (player states)
        if (targetState is States newState)
        {
            state = newState;
        }
    }

    #endregion


    #region RUNNING

    private void Start()
    {
        CharacterStart();
        richBodyTransform = transform.Find("Rich Body");
    }

    #endregion
}
