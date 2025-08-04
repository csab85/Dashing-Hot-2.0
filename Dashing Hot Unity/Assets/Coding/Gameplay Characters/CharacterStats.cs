using Mono.Cecil;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterStats : MonoBehaviour
{
    #region Fields

    // Components
    [HideInInspector] public CharacterPhysics CharPhysics { get; private set; }

    // Stats
    [Header("Base Health Stats")]
    [SerializeField] private float _maxHealth;
    private float _currentHealth;

    [Header("Base Movement Stats")]
    public float Acceleration;
    public float GroundFriction;
    public float FallAcceleration;

    [Header("Battle Stats")]
    public float PushForce;
    public float PushDuration;
    public float DashForce;
    public float DashDuration;
    public float Damage;
    public float Resistance;
    public float FallDuration;
    [HideInInspector] public float ResistanceCoefficient = 5;

    // State flags
    [HideInInspector] public bool IsBreakingObjects;
    [HideInInspector] public bool IsDashing;
    [HideInInspector] public bool IsPropelled;
    [HideInInspector] public bool IsUnstoppable;
    [HideInInspector] public bool IsStunned;

    #endregion

    #region Unity Events

    public event Action OnIdle;
    public event Action OnWalk;
    public event Action OnPropelled; 
    public event Action OnEnableRagdoll;
    public event Action OnDisableRagdoll;

    #endregion

    #region Methods

    public void CallOnIdle()
    {
        OnIdle.Invoke();
    }

    public void CallOnWalk()
    {
        OnWalk.Invoke();
    }

    public void CallOnPropelled()
    {
        OnPropelled.Invoke();
    }

    public void CallOnEnableRagdoll()
    {
        OnEnableRagdoll.Invoke();
    }

    public void CallOnDisableRagdoll()
    {
        OnDisableRagdoll.Invoke();
    }

    protected void CharacterStart()
    {
        CharPhysics = GetComponent<CharacterPhysics>();
        _currentHealth = _maxHealth;
    }

    #endregion
}
