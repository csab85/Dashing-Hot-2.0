using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region VARIABLES

    //COMPONENTS
    Animator animator;
    PlayerStats playerStats;

    //STATES
    PlayerStats.States stateWalking = PlayerStats.States.Walking;
    PlayerStats.States stateIdle = PlayerStats.States.Idle;

    #endregion

    #region RUNNING

    private void Start()
    {
        //get components
        animator = GetComponent<Animator>();
        playerStats = transform.parent.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        //if on walking state
        if (playerStats.state == stateWalking)
        {
            animator.Play("Walking");
        }

        //if on idle state
        if (playerStats.state == stateIdle)
        {
            animator.Play("Idle");
        }
    }

    #endregion
}
