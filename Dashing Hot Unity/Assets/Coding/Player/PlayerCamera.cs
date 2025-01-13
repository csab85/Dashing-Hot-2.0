using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    #region VARS

    //Components
    Transform followTransform;
    CharacterStates playerStates;
    CinemachineFreeLook normalCamera;
    CinemachineVirtualCamera combatCamera;
    Image reticle;

    //stats
    [SerializeField] [Range(0, 1)] float xSensibility;
    [SerializeField][Range(0, 1)] float ySensibility;
    [SerializeField][Range(-80, 80)] float minYAngle;
    [SerializeField][Range(-80, 80)] float maxYAngle;
    
    //move cam with cursor
    float mouseDeltaX = 0;
    float mouseDeltaY = 0;
    float pitch;
    float yaw;
    Quaternion targetRotation;

    #endregion

    #region METHODS

    /// <summary>
    /// Toggles active camera (combat or normal)
    /// </summary>
    /// <param name="context"></param>
    public void ToggleCameraMode(InputAction.CallbackContext context)
    {
        //if on combat mode
        if (playerStates.combatMode)
        {
            //activate normal camera
            normalCamera.Priority = 1;
            combatCamera.Priority = 0;

            //hide reticle
            reticle.enabled = false;

            //update mode
            playerStates.combatMode = false;
        }

        //if on normal mode
        else
        {
            //activate combat camera
            normalCamera.Priority = 0;
            combatCamera.Priority = 1;

            //show reticle
            reticle.enabled = true;

            //update mode
            playerStates.combatMode = true;
        }
    }

    #endregion

    #region RUNNING

    private void Start()
    {
        //set components
        followTransform = GameObject.Find("Follow Target").transform;
        playerStates = GameObject.Find("Player").GetComponent<CharacterStates>();
        normalCamera = GameObject.Find("Normal Camera").GetComponent<CinemachineFreeLook>();
        combatCamera = GameObject.Find("Combat Camera").GetComponent<CinemachineVirtualCamera>();
        reticle = GameObject.Find("Reticle").GetComponent<Image>();

        //Lock mouse to center and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        #region Move camera with cursor (if on combat mode)

        //yaw for player pitch on camera+

        if (playerStates.combatMode)
        {        
            //update mouse delta
            mouseDeltaX = Input.GetAxis("Mouse X") * xSensibility * 5;
            mouseDeltaY = Input.GetAxis("Mouse Y") * ySensibility * 5;

            //if mouse moving
            if (mouseDeltaX != 0 | mouseDeltaY != 0)
            {
                //add delta to yaw and pitch
                yaw += mouseDeltaX;
                pitch -= mouseDeltaY;

                //clamp pitch
                pitch = Mathf.Clamp(pitch, -maxYAngle, -minYAngle);

                //update transform
                targetRotation = Quaternion.Euler(pitch, yaw, 0);
            }

            //lerp towards position
            followTransform.rotation = Quaternion.Slerp(followTransform.rotation, targetRotation, Time.deltaTime * 5);
        }

        #endregion
    }

    #endregion
}
