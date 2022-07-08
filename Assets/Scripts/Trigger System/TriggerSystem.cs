using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSystem : MonoBehaviour
{
    public delegate void OnTriggerActive();
    public OnTriggerActive TriggerActiveEvent;

    public GameObject triggerObj;

    public GameObject pressureWeightObj;        //The Object that the pressure plate requires in order to be activated.

    public enum TriggerType
    {
        Button,
        TimedButton,
        Lever,
        PressurePlate
    }

    public TriggerType triggerType;

    public int timer;

    public bool triggerActive = false;
    public bool triggerHeld = false;

    public bool interactible;

    public bool IsTriggered()
    {
        //switch case to determine if the trigger has been activated.


        //return either triggerActive or triggerHeld
        return triggerActive;
    }

    void TriggerFunction()
    {
        switch (triggerType)
        {
            //----------------------------------------
            case TriggerType.Button:
                if (interactible)
                    triggerActive = true;


                break;
            //----------------------------------------

            //----------------------------------------
            case TriggerType.TimedButton:
                triggerActive = true;
                
                break;
            //----------------------------------------

            //----------------------------------------
            case TriggerType.Lever:
                triggerActive = true;
                
                break;
            //----------------------------------------

            //----------------------------------------
            case TriggerType.PressurePlate:
                triggerActive = true;
                
                break;
            //----------------------------------------
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}