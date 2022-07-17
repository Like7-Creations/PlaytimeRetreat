using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Prerequesets : MonoBehaviour
{
    public TriggerSystem[] canons;
    public UnityEvent onTriggerEnable;
    public UnityEvent onTriggerDisable;

    private bool wasTriggered;

    delegate void onTrigger();
    onTrigger pressureplate;

    void Start()
    {
    }

    void Update()
    {
        //pressureplate();
    }
    
    public void Checktrigger()
    {
        bool trigger = true;
        for (int i = 0; i < canons.Length; i++)
        {
            if (!canons[i].IsTriggered())
            {
                trigger = false;
                break;
            }
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
