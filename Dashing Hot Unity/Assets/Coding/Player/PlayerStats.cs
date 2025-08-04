using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : CharacterStats
{
    #region VARS

    [Header("Player Movement Stats")]
    public float TurnAroundSpeed;

    public bool isOnCombatMode;
    public bool isUsingSkill;

    #endregion

    #region METHODS

    IEnumerator StopDashing()
    {
        yield return new WaitForEndOfFrame();
    }

    public void CallOnDash()
    {
        OnDash.Invoke();
    }

    public void CallOnHitPunch()
    {
        OnHitPunch.Invoke();
    }

    #endregion

    #region EVENTS

    public event Action OnDash;
    public event Action OnHitPunch;

    #endregion

    #region RUNNING

    private void Start()
    {
        CharacterStart();
    }

    #endregion
}
