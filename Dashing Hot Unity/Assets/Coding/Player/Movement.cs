using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{
    #region VARIABLES

    //self components
    Rigidbody rb;
    CharacterStates playerStates;

    //import components
    Transform mainCameraTransform;
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
    CharacterStates.States stateWalking = CharacterStates.States.Walking;
    CharacterStates.States stateIdle = CharacterStates.States.Idle;

    #endregion

    #region METHODS

    /// <summary>
    /// Sets state to walking and gets input direction
    /// </summary>
    /// <param name="context"></param>
    public void Walk(InputAction.CallbackContext context)
    {
        //if movemenbt button started
        if (context.started | context.performed)
        {
            //get input direction
            inputDirection = context.ReadValue<Vector2>();

            //set state to walking
            playerStates.SetState(stateWalking);
        }

        //if movement button released
        if (context.canceled)
        {
            //set input direction to 0
            inputDirection = Vector2.zero;

            //set state to idle
            playerStates.SetState(stateIdle);
        }
    }

    #endregion

    #region RUNNING

    private void Start()
    {
        //Set components
        rb = GetComponent<Rigidbody>();
        playerStates = GetComponent<CharacterStates>();
        mainCameraTransform = GameObject.Find("Main Camera").transform;
        followTrasnform = GameObject.Find("Follow Target").transform;
        meshRootTransform = transform.Find("Rich Body").transform;
    }

    private void FixedUpdate()
    {
        //move if state is walking
        if (playerStates.state == stateWalking)
        {
            //update direction
            direction = inputDirection.x * mainCameraTransform.right + inputDirection.y * mainCameraTransform.forward;
            direction = direction.normalized;
            direction.y = 0;

            //look at direction if on normal mode
            if (!playerStates.combatMode)
            {
                meshRootTransform.rotation = Quaternion.LookRotation(direction);
            }

            //accelerate towards direction
            Vector3 force = direction * acceleration;
            rb.AddForce(force.x, 0, force.z);
        }

        //look forward if on combat mode
        if (playerStates.combatMode)
        {
            Vector3 forwardDirection = new Vector3(followTrasnform.forward.x, 0, followTrasnform.forward.z);
            meshRootTransform.rotation = Quaternion.LookRotation(forwardDirection);
        }
    }

    #endregion
}
