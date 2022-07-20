using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetPosition : MonoBehaviour
{
    public Transform resetPoint;

    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.position = resetPoint.position;
    }
}
