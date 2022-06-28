using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float camSens;
    public Transform player;
    PlayerController playerController;
    Animator anim;
    float rotationX;
    Vector3 lastPosition;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerController = GetComponentInParent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") *Time.deltaTime* camSens;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * camSens;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
       // transform.localEulerAngles = new Vector3(rotationX, 0, 0);
        player.Rotate(Vector3.up * mouseX);


        if (lastPosition != playerController.transform.position)
        {
            //anim.SetBool("Bobbing", true);
        }
       // else anim.SetBool("Bobbing", false);
        lastPosition = playerController.transform.position;
    }
}
