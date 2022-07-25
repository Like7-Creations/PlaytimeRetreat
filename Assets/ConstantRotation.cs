using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public Vector3 rotVelocity;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotVelocity.x * Time.fixedDeltaTime, rotVelocity.y * Time.fixedDeltaTime, rotVelocity.z * Time.fixedDeltaTime);
    }
}
