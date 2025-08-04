using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    #region FIELDS/PROPERTIES

    public IState CurrentState { get; private set; }
    [SerializeField] bool isPlayer;

    #endregion

    #region METHODS

    public void ChangeState(IState newState)
    {
        CurrentState.Exit();
        bool changeSuccessful = newState.Enter();

        //fire event if change successfull
        if (changeSuccessful)
        {
            CurrentState = newState;
        }
    }

    public void ReturnToIdleState()
    {
        if (isPlayer)
        {
            ChangeState(new IdleState.Player(gameObject));
        }

        else
        {
            ChangeState(new IdleState.Enemy(gameObject));
        }
    }

    #endregion

    #region EVENTS

    public event Action<IState> OnStateChanged;

    #endregion

    #region UNITY LIFECYCLE

    private void Start()
    {
        if (isPlayer)
        {
            CurrentState = new IdleState.Player(gameObject);
        }

        else
        {
            CurrentState = new IdleState.Enemy(gameObject);
        }
    }

    private void Update()
    {
        CurrentState.Update();
    }

    private void FixedUpdate()
    {
        CurrentState.FixedUpdate();
    }

    #endregion
}
