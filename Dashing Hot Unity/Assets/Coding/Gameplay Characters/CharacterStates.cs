using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    #region VARS

    public float health;
    public float maxHealth;

    public bool dashing;

    #endregion

    #region METHODS

    public abstract void SetState<TState>(TState targetState);

    #endregion
}
