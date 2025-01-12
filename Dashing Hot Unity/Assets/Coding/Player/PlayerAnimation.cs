using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region VARIABLES

    //COMPONENTS
    Animator animator;
    CharacterStates charStates;

    //STATES
    CharacterStates.States stateWalking = CharacterStates.States.Walking;
    CharacterStates.States stateIdle = CharacterStates.States.Idle;

    #endregion

    #region RUNNING

    private void Start()
    {
        //get components
        animator = GetComponent<Animator>();
        charStates = transform.parent.GetComponent<CharacterStates>();
    }

    private void Update()
    {
        //if on walking state
        if (charStates.state == stateWalking)
        {
            animator.Play("Walking");
        }

        //if on idle state
        if (charStates.state == stateIdle)
        {
            animator.Play("Idle");
        }
    }

    #endregion
}
