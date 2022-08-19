using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TriggerSystem : MonoBehaviour
{
    public MechanicsControl TriggerSys;
    InputAction PressButton;
    InputAction PressTimedButton;
    InputAction PressLever;

    public Vector3 originalPos;     //For testing purposes. Need to replace with Animations later
    public int pressureWeightReq;        //The Object that the pressure plate requires in order to be activated.

    public enum TriggerType
    {
        Button,
        TimedButton,
        Lever,
        PressurePlate
    }

    public TriggerType triggerType;

    Color originalObjColor;

    public bool hasPlayer;

    float timer;
    public float timerDuration;
    public bool timerButtonPressed;

    public bool triggerSpotted;

    public bool triggerActive = false;

    public bool buttonPressed = false;
    bool leverPulled = false;
    bool pressureActive = false;

    //Used for triggers that need to be manually disabled, like Buttons and maybe specific Pressure Plates
    //Set to false for Timed Buttons and Levers that disable themselves without player interference. Or for pressure plates that require an object
    //on it to remain triggered
    //public bool toggleable = true;

    public bool interactible;

    private void Awake()
    {
        TriggerSys = new MechanicsControl();
        this.gameObject.AddComponent<TriggerNetComp>();
    }

    void Start()
    {
        originalObjColor = GetComponent<Renderer>().material.color;
        originalPos = transform.position;
        timer = timerDuration;
    }

    void OnEnable()
    {
        PressButton = TriggerSys.Trigger.InteractButton;
        PressButton.Enable();
        PressButton.performed += ButtonPressed;

        PressTimedButton = TriggerSys.Trigger.InteractTimedButton;
        PressTimedButton.Enable();
        PressTimedButton.performed += TimedButtonPressed;

        PressLever = TriggerSys.Trigger.InteractLever;
        PressLever.Enable();
        PressLever.performed += LeverPulled;
    }

    void OnDisable()
    {
        PressButton.Disable();
        PressTimedButton.Disable();
        PressLever.Disable();
    }

    public bool IsTriggered()
    {
        switch (triggerType)
        {
            //----------------------------------------*
            case TriggerType.Button:

                if (buttonPressed)
                {
                    triggerActive = true;
                   // Debug.Log($"{this.name} of type: {this.triggerType} has been activated");
                    return triggerActive;
                }
                else if (!buttonPressed)
                {
                    triggerActive = false;
                    //Debug.Log($"{this.name} of type: {this.triggerType} has been deactivated");
                    return triggerActive;
                }

                break;
            //----------------------------------------*

            //----------------------------------------*
            case TriggerType.TimedButton:
                if (timerButtonPressed)
                {
                    if (timer == timerDuration)
                    {
                        triggerActive = true;
                        timerButtonPressed = true;
                        //Debug.Log($"{this.name} of type: {this.triggerType} has been activated");
                       // Debug.Log($"{this.name}'s timer has been activated");

                        return triggerActive;
                    }

                    else if (timer == 0)
                    {
                        timer = timerDuration;
                    }
                }

                else if (!timerButtonPressed)
                {
                    if (timer == 0)
                    {
                        triggerActive = false;
                        timerButtonPressed = false;
                        timer = timerDuration;
                       // Debug.Log($"{this.name} of type: {this.triggerType} has been deactivated");

                        return triggerActive;
                    }
                }

                break;
            //----------------------------------------*

            //----------------------------------------
            case TriggerType.Lever:
                if (leverPulled)
                {
                    triggerActive = true;
                    //Debug.Log($"{this.name} of type: {this.triggerType} has been activated");
                    return triggerActive;
                }

                else if (!leverPulled)
                {
                    triggerActive = false;
                    //Debug.Log($"{this.name} of type: {this.triggerType} has been deactivated");
                    return triggerActive;
                }

                break;
            //----------------------------------------

            //----------------------------------------*
            case TriggerType.PressurePlate:
                if (pressureActive)
                {
                    triggerActive = true;
                    //Debug.Log($"{this.name} of type: {this.triggerType} has been activated");
                    return triggerActive;
                }

                else if (!pressureActive)
                {
                    triggerActive = false;
                    //Debug.Log($"{this.name} of type: {this.triggerType} has been deactivated");
                    return triggerActive;
                }

                break;
                //----------------------------------------*
        }

        //switch case to determine if the trigger has been activated.
        //Do if statement that checks trigger type and return coorresponding bool.

        //return either triggerActive or triggerHeld
        return triggerActive;
    }

    public void ButtonPressed(InputAction.CallbackContext context)
    {
        if (hasPlayer)
        {
            if (!buttonPressed)
            {
                buttonPressed = true;
                //Debug.Log("Button Has Been Pressed");
            }

            else if (buttonPressed)
            {
                buttonPressed = false;
                //Debug.Log("Button Has Been Reset");
            }
        }

    }

    public void TimedButtonPressed(InputAction.CallbackContext context)
    {
        if (hasPlayer)
        {
            if (timer == timerDuration)
            {
                timerButtonPressed = true;
                //Debug.Log("TimedButton Has Been Pressed");
            }
        }
    }

    public void LeverPulled(InputAction.CallbackContext context)
    {
        if (hasPlayer)
        {
            if (!leverPulled)
            {
                leverPulled = true;
               // Debug.Log("Lever Has Been Pulled");
            }

            else if (leverPulled)
            {
                leverPulled = false;
               // Debug.Log("Lever Has Been Let Go");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "EffectableObject" || collision.collider.tag == "Player")
        {
            //collision.transform.parent = transform;
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "EffectableObject" || collision.collider.tag == "Player")
        {
            if (collision.rigidbody.mass == pressureWeightReq)
            {
                //transform.Translate(0, -0.1f, 0);
                pressureActive = true;
                //Debug.Log($"A Pressure Plate has been activated");
            }

            else
            {
                pressureActive = false;
                //transform.position = originalPos;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "EffectableObject" || collision.collider.tag == "Player")
        {
            pressureActive = false;
           // Debug.Log($"A Pressure Plate has been deactivated");

            collision.transform.parent = null;
            GetComponent<Renderer>().material.color = originalObjColor;
        }
    }


    void Update()
    {

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2))
        {
            hasPlayer = hit.collider.gameObject == this.gameObject;
        }
        else hasPlayer = false;


        if (triggerType == TriggerType.TimedButton)
        {
            if (triggerActive)
            {
                if (timerButtonPressed)
                {
                    if (timer > 0)
                    {
                        timer -= Time.deltaTime;
                        //Debug.Log($"{timer} seconds left");
                    }

                    else
                    {
                        timer = 0;
                        timerButtonPressed = false;
                       // Debug.Log("Timed Button Reset");
                       // Debug.Log($"{this.name}'s timer has been deactivated");
                    }
                }
            }
        }

        if (triggerType == TriggerType.PressurePlate)
        {
            if (!pressureActive)
            {
                if (transform.position.y < originalPos.y)
                {
                    transform.Translate(0, 0.1f, 0);
                   // Debug.Log("Pressure Plate Reset");
                }

                else
                {
                    pressureActive = true;  //This should make sure that if the object's y pos is lower than original, then the pressure plate is active.
                }
            }
        }
    }
}