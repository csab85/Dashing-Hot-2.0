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

    Rigidbody rb;

    #endregion

    #region EVENTS

    [HideInInspector] public UnityEvent OnDash;
    [HideInInspector]
    


    #endregion

    #region METHODS

    public abstract void SetState<TState>(TState targetState);

    public void PushSelf(Vector3 direction, float velocity, float duration)
    {
        
    }

    #endregion

    #region RUNNING

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    #endregion
}
