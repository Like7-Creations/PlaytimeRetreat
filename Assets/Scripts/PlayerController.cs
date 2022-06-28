using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    public CameraController cam;
  
    public float speed;
    public float jumpHeight;
    public float gravity = -9.81f;
    float moveInputdeadzone;
   
    [SerializeField]bool isGrounded;
    public Vector3 velocity;

    int leftFingerId, rightFingerId;

    float halfscreen;

    Vector2 lookinput;
    float camPitch;

    Vector2 moveTouchstart;
    Vector2 moveInput;
    
    public Vector3 lastCheckpoint;

    void Start()
    {  
        characterController = GetComponent<CharacterController>();
        cam = GetComponentInChildren<CameraController>();
        leftFingerId = -1;
        rightFingerId = -1;
        lastCheckpoint = transform.position;
        halfscreen = Screen.width / 2;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.5f);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        /* if(what ever happens to player here)
         {
             TPToCheckpoint(lastCheckpoint);
         }*/
    }

    void TPToCheckpoint(Vector3 lastpoint)
    {
        transform.position = lastpoint;
    }

}
