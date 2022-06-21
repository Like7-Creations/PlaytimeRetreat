using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] float ForceHeight;
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("launchpad??");
            playerController.velocity.y = Mathf.Sqrt(ForceHeight * -2f * playerController.gravity);
        }
    }
}
