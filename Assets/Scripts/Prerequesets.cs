using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Prerequesets : MonoBehaviour
{
    public ConveyorButton[] canons;
    /*public UnityEvent onTrigger;
    public UnityEvent onTriggerEnable;
    public UnityEvent onTriggerDisable;*/

    delegate void onTrigger();
    onTrigger pressureplate;

    void Start()
    {
        pressureplate += pressureplatePressed;
        pressureplate += playerfarts;
    }

    void Update()
    {
        pressureplate();
    }
    
    public void Checktrigger()
    {
        bool trigger = true;
        for (int i = 0; i < canons.Length; i++)
        {
            //if(canons[i] = true)
            // invoke the event
        }
    }

    public void pressureplatePressed()
    {
        Debug.Log("prssure plate pressed");
    }

    public void playerfarts()
    {
        Debug.Log("prssure plate farted");
    }
}
