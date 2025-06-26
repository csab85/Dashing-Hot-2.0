using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterStats : MonoBehaviour
{
    #region VARS

    public float health;
    public float maxHealth;

    public bool dashing;
    public bool yeeted;
    public bool stunned;

    #endregion

    #region EVENTS

    [HideInInspector] public UnityEvent OnDash;
    [HideInInspector]
    


    #endregion

    #region METHODS

    public abstract void SetState<TState>(TState targetState);

    #endregion
}
