using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    PlayerController playerController;
    MobileController mobileController;
    [SerializeField] float ForceHeight;
    AudioSource audioSource;
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        mobileController = FindObjectOfType<MobileController>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audioSource.Play();
            playerController.velocity.y = Mathf.Sqrt(ForceHeight * -2f * playerController.gravity);
            mobileController.velocity.y = Mathf.Sqrt(ForceHeight * -2f * playerController.gravity);
        }
    }
}
