using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    #region VARIABLES

    //Player stats
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float maxVelocity;

    //VARS
    Vector2 direction;
    float accEscalated;

    //STATES
    CharacterStates.States stateWalking= CharacterStates.States.Walking;
    CharacterStates.States stateIdle = CharacterStates.States.Idle;

    //components
    Rigidbody rb;
    CharacterStates charStates;

    #endregion

    #region METHODS

    public void Walk(InputAction.CallbackContext context)
    {
        //if movemenbt button started
        if (context.started)
        {
            //update direction and escalate acceleration
            direction = context.ReadValue<Vector2>();
            accEscalated = acceleration;

            //set state to walking
            charStates.SetState(stateWalking);
        }

        //if movement button released
        if (context.canceled)
        {
            //set state to idle
            charStates.SetState(stateIdle);
        }
    }

    #endregion

    #region RUNNING

    private void Start()
    {
        //Set components
        rb = GetComponent<Rigidbody>();
        charStates = GetComponent<CharacterStates>();
    }

    private void FixedUpdate()
    {
        //move if state is walking
        if (charStates.state == stateWalking)
        {
            //accelerate towards direction
            Vector2 fDirection = direction * accEscalated;
            rb.AddForce(fDirection.x, 0, fDirection.y);

            print(direction);
        }
    }

    #endregion
}
