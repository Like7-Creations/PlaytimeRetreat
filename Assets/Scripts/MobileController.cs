using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobileController : MonoBehaviour
{
    CharacterController characterController;
    public CameraController cam;
    PickUpThrow[] pickupthrow;
    [SerializeField] Button pickUpBut;
    [SerializeField] Button ThrowBut;
    //[SerializeField] Button jumpButt;

    public float speed;
    public float jumpHeight;
    public float gravity = -9.81f;
    float moveInputdeadzone;

    [SerializeField] bool isGrounded;
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
        pickupthrow = FindObjectsOfType<PickUpThrow>();
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


        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        //jumpButt.onClick.AddListener(Jump);

        /* if(what ever happens to player here)
         {
             TPToCheckpoint(lastCheckpoint);
         }*/

        for (int i = 0; i < pickupthrow.Length; i++)
        {
            if (pickupthrow[i].hasplayer)
            {
                pickUpBut.gameObject.SetActive(true);
            }
            else { pickUpBut.gameObject.SetActive(false); }
            
            if (pickupthrow[i].holding)
            {
                ThrowBut.gameObject.SetActive(true);
                pickUpBut.gameObject.SetActive(false);
            }
            else
            {
                ThrowBut.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (t.position.x < halfscreen && leftFingerId == -1)
                    {
                        leftFingerId = t.fingerId;
                        moveTouchstart = t.position;
                    }
                    else if (t.position.x > halfscreen && rightFingerId == -1)
                    {
                        rightFingerId = t.fingerId;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:

                    if (t.fingerId == leftFingerId)
                    {
                        leftFingerId = -1;
                    }
                    else if (t.fingerId == rightFingerId)
                    {
                        rightFingerId = -1;
                    }
                    break;

                case TouchPhase.Moved:
                    if (t.fingerId == rightFingerId)
                    {
                        lookinput = t.deltaPosition * cam.camSens * Time.deltaTime;
                    }
                    else if (t.fingerId == leftFingerId)
                    {
                        moveInput = t.position - moveTouchstart;
                    }
                    break;
                case TouchPhase.Stationary:
                    if (t.fingerId == rightFingerId)
                    {
                        lookinput = Vector2.zero;
                    }
                    break;
            }
        }

        if (rightFingerId != -1)
        {
            LookAround();
        }
        if (leftFingerId != -1)
        {
            Move();
        }
    }

    void LookAround()
    {
        camPitch = Mathf.Clamp(camPitch - lookinput.y, -90, 90);
        cam.transform.localRotation = Quaternion.Euler(camPitch, 0, 0);

        transform.Rotate(transform.up, lookinput.x);
    }

    void Move()
    {
        if (moveInput.sqrMagnitude <= moveInputdeadzone) return;

        Vector2 movementDirection = moveInput.normalized * speed * Time.deltaTime;
        characterController.Move(transform.right * movementDirection.x + transform.forward * movementDirection.y);
    }

    void Jump()
    {
        if (isGrounded) { velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); }
    }

    void TPToCheckpoint(Vector3 lastpoint)
    {
        transform.position = lastpoint;
    }
}
