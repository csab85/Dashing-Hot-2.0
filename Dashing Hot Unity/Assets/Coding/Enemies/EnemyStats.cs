using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStats : CharacterStats
{
    #region VARS

    //IMPORTS
    Rigidbody rb;

    //STATES
    public enum States
    {
        Idle,
        Attacking,
        Vulnerable,
        VulnerableTransition,
        GettingUp,
        Unstunning
    }

    public States state;

    #endregion

    #region EVENTS

    private void OnCollisionEnter(Collision collision)
    {
        //if collision has character stats
        if (collision.gameObject.GetComponent<CharacterStats>() is CharacterStats characterStats)
        {
            if (characterStats.dashing)
            {
                dashDirection = characterStats.dashDirection;
                dashing = true;
                yeeted = true;
            }
        }
    }

    #endregion

    #region METHODS

    public override void SetState<TState>(TState targetState)
    {
        if (targetState is States newState)
        {
            state = newState;
        }
    }


    #endregion

    #region RUNNING

    public void EnemyStart()
    {
        CharacterStart(); 
        CharacterStart(); 
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (yeeted)
        {
            rb.linearVelocity = dashDirection;
        }
    }


    #endregion
}
