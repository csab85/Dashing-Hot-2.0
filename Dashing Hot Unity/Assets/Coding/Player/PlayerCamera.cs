using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    #region VARS

    //Imports
    Transform followTransform;

    //stats
    [SerializeField] [Range(0, 1)] float xSensibility;
    [SerializeField][Range(0, 1)] float ySensibility;

    float mouseDeltaX = 0;
    float mouseDeltaY = 0;

    #endregion

    #region RUNNING

    private void Start()
    {
        followTransform = GameObject.Find("Follow Target").transform;
    }

    private void Update()
    {
        //update mouse delta
        mouseDeltaX = Input.GetAxis("Mouse X");
        mouseDeltaY = Input.GetAxis("Mouse Y");

        //if mouse moving
        if (mouseDeltaX != 0 | mouseDeltaX != 0)
        {
            //apply sensibility
            float xRotation = mouseDeltaX * xSensibility;
            float yRotation = mouseDeltaY * ySensibility;

            //rotate follow obj
            followTransform.Rotate(new Vector3(0, xRotation, 0), Space.World);
            followTransform.Rotate(new Vector3(-yRotation, 0, 0), Space.Self);
        }
    }

    #endregion
}
