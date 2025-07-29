using UnityEngine;

public class EnemyStats : CharacterStats
{
    #region

    enum States
    {
        Idle,
        OnAir,
        KnockedDown
    }

    [SerializeField] States state = States.Idle;

    #endregion

    #region METHODS

    public override void SetState<TState>(TState targetState)
    {
        if (targetState is States newState)
        {
            state = newState;
        }
    }

    #endregion
}
