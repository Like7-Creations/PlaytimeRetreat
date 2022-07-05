using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField, Range (0,5)] float forceMag = 1;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rBody = hit.collider.attachedRigidbody;

        if (rBody != null)
        {
            Vector3 forceDir = hit.gameObject.transform.position - transform.position;
            forceDir.y = 0;
            forceDir.Normalize();

            rBody.AddForceAtPosition(forceDir * forceMag, transform.position, ForceMode.Impulse);
        }
    }
}
