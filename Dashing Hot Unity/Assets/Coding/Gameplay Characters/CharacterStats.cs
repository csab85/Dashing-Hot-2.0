using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    #region VARS

    //components
    Rigidbody rb;
    float rbDrag;

    public float health;
    public float maxHealth;
    public float damage;
    public float speed;
    public float stunDuration;
    public float yeetDuration;
    public float dashDuration;
    public Vector3 dashDirection;

    public bool dashing;//is dashing towards a direction, and can damage and destroy scenario
    public bool yeeted;// was thrown in the air (if not yeeted but dashing, means it is attacking)
    public bool stunned;//stays with defenses open for a period of time
    public bool knocked;//fell down and stays with defenses open for a period of time. Gotta get up later
    public bool airbone;//is in the air, can do nothing
    public bool pushing;//while dashing, won't be stopped and will push other characters away

    #endregion

    #region METHODS

    public abstract void SetState<TState>(TState targetState);

    //SE PA TRANSFORMAR EM UMA FUNCAO SEPARADA QUE CHAMA ESSA CORROTINA, NA HORA DE ANIMAR
    public IEnumerator StartDashing(bool yeeted = false)
    {
        //stop other coroutines
        StopAllCoroutines();

        //set dashing state
        dashing = true;

        //set yeeted true if yeeted
        if (yeeted) yeeted = true;

        //set drag to 0
        rb.linearDamping = 0;

        //shoot dashing event signal
        OnDashStart?.Invoke();

        //wait for dash duration
        yield return new WaitForSeconds(dashDuration);
        
        //start stop dashing coroutine
        StartCoroutine(StopDashing());
    }

    public IEnumerator StopDashing()
    {
        //wait till end of frame
        yield return new WaitForEndOfFrame();

        //Stop other coroutines
        StopAllCoroutines();

        //send stop dashing event signal
        OnDashStop?.Invoke();

        //stop dashing and yeeted
        dashing = false;
        yeeted = false;

        //set drag to normal
        rb.linearDamping = rbDrag;
    }

    #endregion

    #region EVENTS

    //SIGNAL EVENTS
    public static event System.Action OnDashStart;
    public static event System.Action OnDashStop;

    private void OnCollisionEnter(Collision collision)
    {
        //if collision has character stats
        if (collision.gameObject.GetComponent<CharacterStats>() is CharacterStats collisionStats)
        {
            //if collision is dashing
            if (collisionStats.dashing)
            {
                //if self is not dashing
                if (!dashing)
                {
                    //if collision is pushing
                    if (collisionStats.pushing)
                    {
                        //set dashing direction as if it was pushed by collision
                        dashDirection = (transform.position - collision.transform.position).normalized * 10;
                    }

                    //if collision isnt pushing
                    else
                    {
                        //set dashing direction as same of the collision
                        dashDirection = collisionStats.dashDirection;
                    }

                    //yeet self
                    StartCoroutine(StartDashing(true));
                }

                //if self is dashing
                else
                {
                    //if self is yeeted
                    if (yeeted)
                    {
                        //if collision is yeeted
                        if (collisionStats.yeeted)
                        {
                            //if collision is pushing
                            if (collisionStats.pushing)
                            {
                                //set dashing direction as if it was pushed by collision
                                dashDirection = (transform.position - collision.transform.position).normalized * 10;
                            }

                            //if collision is not pushing
                            else
                            {
                                //stop yeet
                                StartCoroutine(StopDashing());
                            }
                        }

                        //if collision is not yeeted
                        else
                        {
                            //if collision is pushing
                            if (collisionStats.pushing)
                            {
                                //set dashing direction as if it was pushed by collision
                                dashDirection = (transform.position - collision.transform.position).normalized * 10;
                            }

                            //if collision is not pushing
                            else
                            {
                                //set dashing direction as same of the collision
                                dashDirection = collisionStats.dashDirection;
                            }
                        }
                    }

                    //if self is not yeeted
                    else
                    {
                        //if not pushing
                        if (!pushing)
                        {
                            StartCoroutine(StopDashing());
                        }
                    }
                }
            }

            //if collision is not dashing
            else
            {
                //if self is dashing
                if (dashing)
                {
                    //if self is not pushing
                    if (!pushing)
                    {
                        StartCoroutine(StopDashing());
                    }
                }
            }
        }
    }

    #endregion

    #region RUNNING

    public void CharacterStart()
    {
        rb = GetComponent<Rigidbody>();
        rbDrag = rb.linearDamping;
    }

    private void Update()
    {
        if (dashing)
        {
            rb.linearVelocity = dashDirection * speed;
        }
    }

    #endregion
}

