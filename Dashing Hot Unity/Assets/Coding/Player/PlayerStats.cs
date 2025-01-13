using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    #region STATES

    public enum States
    {
        Idle,
        Walking
    }

    public States state;

    public bool combatMode;

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
}
