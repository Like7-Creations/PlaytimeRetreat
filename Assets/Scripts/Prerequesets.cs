using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Prerequesets : MonoBehaviour
{
    public TriggerSystem[] Triggers;

    public UnityEvent onTriggerEnable;
    public UnityEvent onTriggerDisable;


    private bool wasTriggered;

    //void Awake()
    //{
    //    onTriggerDisable.Remove(onTriggerDisable[0]);
    //    onTriggerEnable.Remove(onTriggerEnable[0]);
    //}
    void Start()
    {
    }

    void Update()
    {
        //pressureplate();
        Checktrigger();
    }
    
    public void Checktrigger()
    {
        bool trigger = true;
        for (int i = 0; i < Triggers.Length; i++)
        {
            if (!Triggers[i].IsTriggered())
            {
                trigger = false;
                break;
                //onTriggerEnable.Invoke();
            }
            //else if (!canons[i].IsTriggered())
            //{
            //    onTriggerDisable.Invoke();
            //}
        }
        if (trigger && !wasTriggered)
        {
            onTriggerEnable.Invoke();
            wasTriggered = true;
        }
        else if (!trigger && wasTriggered)
        {
            onTriggerDisable.Invoke();
            wasTriggered= false;
        }
    }
}
