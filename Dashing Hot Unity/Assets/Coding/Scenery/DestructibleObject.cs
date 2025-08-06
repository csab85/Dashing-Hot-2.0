using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    #region VARS

    float _punchForce;

    #endregion

    #region EVENTS

    private void OnCollisionEnter(Collision collision)
    {
        //if has character script
        if (collision.gameObject.GetComponent<CharacterStats>() is CharacterStats characterStats)
        {
            //if character is breaking objects
            if (characterStats.IsBreakingObjects)
            {
                //activate broke version and unchild
                GameObject brokeVersion = transform.GetChild(0).gameObject;
                brokeVersion.SetActive(true);
                brokeVersion.transform.parent = null;

                //add explosion force to each piece
                foreach (Transform piece in brokeVersion.transform)
                {
                    _punchForce = transform.GetComponent<Rigidbody>().linearVelocity.magnitude;

                    piece.GetComponent<Rigidbody>().AddExplosionForce(_punchForce * 30, collision.transform.position, 10);

                }

                print(collision.gameObject.name + _punchForce);


                //destroy self
                Destroy(gameObject);
            }
        }
    }

    #endregion
}
