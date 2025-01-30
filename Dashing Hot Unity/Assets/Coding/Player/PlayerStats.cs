using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : CharacterStats
{
    #region VARS

    //IMPORTS
    Rigidbody rb;
    Transform richBodyTransform;
    float rbDrag;

    //STATS
    [SerializeField] float speed;

    //STATES
    public enum States
    {
        Idle,
        Walking
    }

    public States state;

    public bool combatMode;
    public bool usingSkill;

    //OTHER
    [HideInInspector] public Vector3 dashDirection;

    #endregion

    #region EVENTS

    private void OnCollisionEnter(Collision collision)
    {
        ////colliding with enemies (change from character stats to enemy stats)
        //if (collision.gameObject.GetComponent<CharacterStats>() is CharacterStats characterStats)
        //{
        //    if (dashing)
        //    {
        //        dashing = false;
        //    }
        //}
    }

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

    IEnumerator StopDashing()
    {
        yield return new WaitForEndOfFrame();
        dashing = false;
    }

    #endregion

    #region RUNNING

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        richBodyTransform = transform.Find("Rich Body");
        rbDrag = rb.drag;
    }

    private void Update()
    {
        if (dashing)
        {
            rb.drag = 0;
            rb.velocity = dashDirection * speed;
        }

        else
        {
            rb.drag = rbDrag;
        }
    }

    #endregion
}
