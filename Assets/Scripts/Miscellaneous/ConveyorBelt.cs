using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject conveyor;
    public Transform beltEndPoint;

    public bool beltActive;

    public float currentBeltSpeed;
    public float maxBeltSpeed;
    
    public void BeltSpeed()
    {
        if (currentBeltSpeed >= maxBeltSpeed)
        {
            currentBeltSpeed = 1;
            Debug.Log("BeltSpeed Reset");
        }

        else
        {
            currentBeltSpeed++;
            Debug.Log("BeltSpeed Increased By 1");
        }
    }

    public void BeltPower()
    {
        if (beltActive)
        {
            beltActive = false;
            Debug.Log("Belt Off");
        }

        else
        {
            beltActive = true;
            Debug.Log("Belt On");
        }
    }

    public void SwitchBeltDirection()
    {
        beltEndPoint.transform.forward = -beltEndPoint.transform.forward;
    }

    private void OnTriggerStay(Collider other)
    {
        if (beltActive)
            other.transform.Translate(beltEndPoint.transform.forward * currentBeltSpeed * Time.deltaTime, Space.World);
    }
}