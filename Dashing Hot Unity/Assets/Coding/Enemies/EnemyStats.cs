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


    #endregion

    #region RUNNING

    private void Start()
    {
        CharacterStart();
    }

    #endregion
}
