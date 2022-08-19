using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    PlayerController playerController;
    MobileController mobileController;
    [SerializeField] float ForceHeight;
    AudioSource audioSource;
    [SerializeField] bool jumped;
    float timer;

    public bool active;
    public Color activeColor, inactiveColor;
    public Renderer[] activeIndicators;

    public Animator ani;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        mobileController = FindObjectOfType<MobileController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (jumped) timer += Time.deltaTime;
        if(timer > 1 && playerController.isGrounded) {playerController.velocity = Vector3.zero; timer = 0; jumped = false; }

        if (active)
        {
            for (int i = 0; i < activeIndicators.Length; i++)
            {
                Material mat = activeIndicators[i].material;
                mat.color = activeColor;
                activeIndicators[i].material = mat;
            }
        }
        else
        {
            for (int i = 0; i < activeIndicators.Length; i++)
            {
                Material mat = activeIndicators[i].material;
                mat.color = inactiveColor;
                activeIndicators[i].material = mat;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && active)
        {
            playerController.velocity = transform.up * ForceHeight;
            jumped = true;


            // Mobile
            mobileController.velocity.y = Mathf.Sqrt(ForceHeight * -2f * playerController.gravity);

            ani.SetTrigger("Launch");
            audioSource.Play();
        }
        
    }

    public void Trigger()
    {
        active = !active;
    }
}
