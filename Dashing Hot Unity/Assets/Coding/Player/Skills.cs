using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    #region VARS

    //imports
    PlayerStats playerStats;
    Transform richBodyTransform;

    #endregion

    #region METHODS

    /// <summary>
    /// Runs DashCoroutine if player is in combat mode and can use skills
    /// </summary>
    /// <param name="context"></param>
    public void Dash(InputAction.CallbackContext context)
    {
        //if on combat mode
        if (playerStats.combatMode)
        {
            //if not using skill and not stunned
            if (!playerStats.usingSkill && !playerStats.stunned)
            {
                //update dash direction based on mesh
                playerStats.dashDirection = richBodyTransform.forward;

                //set dashing and using skill to true
                StartCoroutine(playerStats.StartDashing());
                playerStats.usingSkill = true;
            }
        }
    }

    void ResetUsingSkill()
    {
        playerStats.usingSkill = false;
    }

    #endregion

    #region EVENTS

    private void OnCollisionEnter(Collision collision)
    {
        if (playerStats.dashing && !playerStats.yeeted)
        {
            if (collision.gameObject.GetComponent<CharacterStats>() is CharacterStats targetStats)
            {
                targetStats.yeeted = true;
                targetStats.dashDirection = playerStats.dashDirection;
            }
        }
    }

    #endregion

    #region RUNNING

    private void Start()
    {
        //set imports
        playerStats = GetComponent<PlayerStats>();
        richBodyTransform = transform.Find("Rich Body");

        //add event listeners
        CharacterStats.OnDashStop += ResetUsingSkill;
    }

    #endregion
}
