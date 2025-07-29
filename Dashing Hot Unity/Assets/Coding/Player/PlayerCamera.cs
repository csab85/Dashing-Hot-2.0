using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    #region VARS

    //Components
    Transform followTransform;
    PlayerStats playerStats;
    CinemachineCamera normalCamera;
    CinemachineCamera combatCamera;
    Image reticle;

    //stats
    [SerializeField] [Range(0, 100)] float xSensibility;
    [SerializeField][Range(0, 100)] float ySensibility;
    [SerializeField][Range(-80, 80)] float minYAngle;
    [SerializeField][Range(-80, 80)] float maxYAngle;
    
    //move cam with cursor
    float mouseDeltaX = 0;
    float mouseDeltaY = 0;
    float pitch;
    float yaw;
    Vector3 cameraRotation;
    Quaternion targetRotation;

    #endregion

    #region METHODS

    IEnumerator ActivateCOmbatCamera()
    {
        yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// Toggles active camera (combat or normal)
    /// </summary>
    /// <param name="context"></param>
    public void ToggleCameraMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //if on combat mode
            if (playerStats.combatMode)
            {
                //activate normal camera
                normalCamera.Priority = 1;
                combatCamera.Priority = 0;

                //hide reticle
                reticle.enabled = false;

                //update mode
                playerStats.combatMode = false;
            }

            //if on normal mode
            else
            {
                //update vars based on last camera position
                cameraRotation = Camera.main.transform.rotation.eulerAngles;

                yaw = cameraRotation.y;
                pitch = cameraRotation.x;
                if (pitch > 180) pitch -= 360; //fix pitch wrap around
                pitch = Mathf.Clamp(pitch, -maxYAngle, -minYAngle); //clamp pitch

                targetRotation = Quaternion.Euler(pitch, yaw, 0);

                //update follow transform so it inherits last cam posit
                followTransform.rotation = targetRotation;

                //activate combat camera
                normalCamera.Priority = 0;
                combatCamera.Priority = 1;

                //show reticle
                reticle.enabled = true;

                //update mode
                playerStats.combatMode = true;
            }
        }
    }

    #endregion

    #region RUNNING

    private void Start()
    {
        //set components
        followTransform = GameObject.Find("Follow Target").transform;
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        normalCamera = GameObject.Find("Normal Camera").GetComponent<CinemachineCamera>();
        combatCamera = GameObject.Find("Combat Camera").GetComponent<CinemachineCamera>();
        reticle = GameObject.Find("Reticle").GetComponent<Image>();

        //Lock mouse to center and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        #region Move camera (with follow transform) with cursor

        //yaw for player pitch on camera+

        if (playerStats.combatMode)
        {
            //update mouse delta
            mouseDeltaX = Input.GetAxis("Mouse X") * xSensibility * Time.deltaTime;
            mouseDeltaY = Input.GetAxis("Mouse Y") * ySensibility * Time.deltaTime;

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

            followTransform.rotation = Quaternion.Slerp(followTransform.rotation, targetRotation, Time.deltaTime * 5);
        }

        #endregion
    }

    #endregion
}
