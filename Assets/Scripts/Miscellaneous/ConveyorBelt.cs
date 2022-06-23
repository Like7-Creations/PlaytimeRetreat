using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject conveyor;
    public GameObject beltPowerSwitch;
    //public GameObject beltSpeedSwitch;
    public GameObject buttonPressed;

    GameObject playerCam;

    public Transform beltEndPoint;
    //public Rigidbody rBody;

    public float currentBeltSpeed;
    //public float maxBeltSpeed;
    public bool beltActive;

    private void Start()
    {
        //rBody = GetComponent<Rigidbody>();

        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
        buttonPressed = null;
    }

    private void Update()
    {
        /*for (int i = 0; i < objsOnBelt.Count - 1; i++)
        {
            objsOnBelt[i].GetComponent<Rigidbody>().velocity = speed * direction * Time.deltaTime;
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            if (CheckForButton())
            {
                if (buttonPressed = beltPowerSwitch)
                {
                    BeltPower();
                }

                /*else if (buttonPressed = beltSpeedSwitch)
                {
                    BeltSpeed();
                }*/
            }
        }
    }

    /*void BeltSpeed()
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
    }*/

    void BeltPower()
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

    bool CheckForButton()
    {
        bool rayHitButton = false;

        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = playerCam.GetComponent<Camera>().ScreenPointToRay(new Vector2(x, y));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 5))
        {
            if (hit.collider.gameObject == beltPowerSwitch)
            {
                rayHitButton = true;
                buttonPressed = beltPowerSwitch;
                Debug.Log("Power Switch Pressed.");
            }

            /*else if (hit.collider.gameObject == beltSpeedSwitch)
            {
                rayHitButton = true;
                buttonPressed = beltSpeedSwitch;
                Debug.Log("Speed Switch Pressed.");
            }*/
        }

        return rayHitButton;
    }

    private void OnTriggerStay(Collider other)
    {
        if (beltActive)
            other.transform.Translate(beltEndPoint.transform.forward * currentBeltSpeed * Time.deltaTime, Space.World);
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        objsOnBelt.Add(collision.gameObject);

        collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        objsOnBelt.Remove(collision.gameObject);
    }

    // FixedUpdate is called every fixed framerate frame, if MonoBehaviour is enabled
    void FixedUpdate()
    {
        Vector3 pos = rBody.position;
        rBody.position += Vector3.back * speed * Time.fixedDeltaTime;
        rBody.MovePosition(pos);
    }*/
}
