using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{
    #region VARIABLES

    //imports
    Transform followTrasnform;
    Transform meshRootTransform;

    //Player stats
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float maxVelocity;

    //VARS
    Vector2 inputDirection;
    Vector3 direction;
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
        if (context.started | context.performed)
        {
            //get input direction
            inputDirection = context.ReadValue<Vector2>();

            //set state to walking
            charStates.SetState(stateWalking);
        }

        //if movement button released
        if (context.canceled)
        {
            //set input direction to 0
            inputDirection = Vector2.zero;

            //set state to idle
            charStates.SetState(stateIdle);
        }
    }

    #endregion

    #region RUNNING

    private void Start()
    {
        //Set imports
        followTrasnform = GameObject.Find("Follow Target").transform;
        meshRootTransform = transform.Find("Rich Body").transform;

        //Set components
        rb = GetComponent<Rigidbody>();
        charStates = GetComponent<CharacterStates>();

        //lock cursor (POR NO SCRIPT D GAMEPLAY OU CAMERA DPS)
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        //move if state is walking
        if (charStates.state == stateWalking)
        {
            //update direction
            direction = inputDirection.x * followTrasnform.right + inputDirection.y * followTrasnform.forward;
            direction = direction.normalized;
            direction.y = 0;

            //look at direction
            meshRootTransform.rotation = Quaternion.LookRotation(direction);

            //accelerate towards direction
            Vector3 force = direction * acceleration;
            rb.AddForce(force.x, 0, force.z);
        }
    }

    #endregion
}
