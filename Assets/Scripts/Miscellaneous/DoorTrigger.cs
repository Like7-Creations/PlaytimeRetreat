using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Animator doorAnim = null;

    [SerializeField] bool doorOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!doorOpen)
            {
                doorAnim.Play("Door_Open", 0, 0.0f);
                doorOpen = true;
                Debug.Log("Door Opened");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (doorOpen)
        {
            doorAnim.Play("Door_Close", 0, 0.0f);
            doorOpen = false;
            Debug.Log("Door Closed");
        }
    }
}
