using Cinemachine;
using Cinemachine.Editor;
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

    //stats
    [SerializeField] float dashDuration;

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

                //start coroutine
                StartCoroutine("DashCoroutine");
            }
        }
    }

    /// <summary>
    /// Sets player dashing and using skill to true for dashDuration seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator DashCoroutine()
    {
        //set dashing and using skill to true
        playerStats.dashing = true;
        playerStats.usingSkill = true;

        //wait for dash duration
        yield return new WaitForSeconds(dashDuration);

        //set dashing and using skill to false 
        playerStats.dashing = false;
        playerStats.usingSkill = false;
    }

    #endregion

    #region RUNNING

    private void Start()
    {
        //set imports
        playerStats = GetComponent<PlayerStats>();
        richBodyTransform = transform.Find("Rich Body");
    }

    #endregion
}
