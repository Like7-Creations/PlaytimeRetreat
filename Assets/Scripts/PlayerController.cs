using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public CameraController cam;

    public int level;
  
    public float speed;
    public float jumpHeight;
    public float gravity = -9.81f;

    public bool isGrounded;
    public Vector3 velocity;

    KeyCode[] key = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
    int AbilityIndex;

    public Vector3 lastCheckpoint;

    void Start()
    {  
        characterController = GetComponent<CharacterController>();
        cam = GetComponentInChildren<CameraController>();
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
        for (int i = 0; i < key.Length; i++)
        {
            if (Input.GetKeyDown(key[i]))
            {
                AbilityIndex = i + 1;
                Debug.Log(AbilityIndex);
            }
        }
    }

    void TPToCheckpoint(Vector3 lastpoint)
    {
        transform.position = lastpoint;
    }
}
