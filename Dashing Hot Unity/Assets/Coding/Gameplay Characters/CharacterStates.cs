using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    #region STATES

    public enum States
    {
        Idle,
        Walking
    }

    public States state;

    #endregion

    #region METHODS

    public void SetState(States newState)
    {
        state = newState;
    }

    #endregion
}
