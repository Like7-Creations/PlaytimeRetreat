using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Prerequesets : MonoBehaviour
{
    public ConveyorButton[] canons;
    public UnityEvent onTrigger;
    public UnityEvent onTriggerEnable;
    public UnityEvent onTriggerDisable;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void Checktrigger()
    {
        bool trigger = true;
        for (int i = 0; i < canons.Length; i++)
        {
            //if(canons[i].)
        }
    }
}
