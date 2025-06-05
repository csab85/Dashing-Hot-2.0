using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    #region VARS

    BoxCollider boxCollider;

    [SerializeField] float punchForce;

    #endregion

    #region EVENTS

    private void OnCollisionEnter(Collision collision)
    {
        //if has character script
        if (collision.gameObject.GetComponent<CharacterStats>() is CharacterStats characterStats)
        {
            //if character is dashing
            if (characterStats.dashing)
            {
                //activate broke version and unchild
                GameObject brokeVersion = transform.GetChild(0).gameObject;
                brokeVersion.SetActive(true);
                brokeVersion.transform.parent = null;

                //get contact point
                Vector3 contactPoint = collision.contacts[0].point;
                
                //add explosion force to each piece
                foreach(Transform piece in brokeVersion.transform)
                {
                    Vector3 direction = (piece.position - contactPoint).normalized;

                    piece.gameObject.GetComponent<Rigidbody>().AddForce(-(direction * punchForce), ForceMode.Impulse);
                }

                //destroy self
                Destroy(gameObject);
            }
        }
    }

    #endregion

    #region RUNNING

    private void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
    }

    #endregion
}
