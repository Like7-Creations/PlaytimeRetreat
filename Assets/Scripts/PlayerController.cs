using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GamePackets;
using System;
public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public CameraController cam;

    public TestNetManager tnManager;
    public PlayerNetComp pcNetComp;

    [SerializeField] string testString;

    //public enum PlayerType
    //{
    //    Local,
    //    Partner
    //}

    //public PlayerType playerType;

    public int level;

    public float speed;
    public float jumpHeight;
    public float gravity = -9.81f;

    public bool isGrounded;
    public Vector3 velocity;
    public Vector3 movement;

    [SerializeField] int id;

    KeyCode[] key = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
    int AbilityIndex;

    public Vector3 lastCheckpoint;

    public Guid testGuid;

    public PlayerController[] pcontrollers;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        tnManager = FindObjectOfType<TestNetManager>();
        pcNetComp = gameObject.GetComponent<PlayerNetComp>();

        testGuid = tnManager.PlayerId;
        //testGuid = pcNetComp.localID;
        pcNetComp.localID = testGuid;
        testString = testGuid.ToString();

        pcontrollers = FindObjectsOfType<PlayerController>();


        if (pcontrollers.Length > 1) { testGuid = tnManager.PartnerGuidStore; testString = testGuid.ToString(); }

        /*if (pcNetComp.localID == tnManager.PlayerId)
        {
            cam = GetComponentInChildren<CameraController>();
            cam.gameObject.SetActive(true);
        }*/
    }

    void Update()
    {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.5f);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        print(testGuid);
        if (testGuid != null)
        {
            if (testGuid == pcNetComp.localID)
            {
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                movement = transform.right * x + transform.forward * z; ;
                //Vector3 move = transform.right * x + transform.forward * z;
                //movement = move;

                characterController.Move(movement * speed * Time.deltaTime);

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
        }

    }

    void TPToCheckpoint(Vector3 lastpoint)
    {
        transform.position = lastpoint;
    }
}
