using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAim : MonoBehaviour
{
    public LayerMask freezeable;

    public bool freezeAim;

    public Transform targetObj;
    public FreezeObj[] freezableObjects;

    public Color selectedColor;
    public Color frozenColor;
    public Color finalColor;

    void Start()
    {
        Cursor.visible = false;


    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            FreezeTarget(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            FreezeTarget(false);
        }



        /*
        if (freezeAim)
        {
            RaycastHit hit;

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, freezeable))
            {
                if (targetObj != hit.transform)
                {
                    if (targetObj != null && targetObj.GetComponent<FreezeObj>() != null)
                        targetObj.GetComponent<Renderer>().material.SetColor("_EmissionColor", frozenColor);

                    targetObj = hit.transform;

                    if (targetObj.GetComponent<FreezeObj>() != null)
                    {
                        if (!targetObj.GetComponent<FreezeObj>().freezeActive)
                            targetObj.GetComponent<Renderer>().material.SetColor("_EmissionColor", selectedColor);
                    }
                }
            }

            else
            {
                if (targetObj != null)
                {
                    if (targetObj.GetComponent<FreezeObj>() != null)
                        if (!targetObj.GetComponent<FreezeObj>().freezeActive)
                            targetObj.GetComponent<Renderer>().material.SetColor("_EmissionColor", frozenColor);

                    targetObj = null;

                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (freezeAim)
                {
                    if (targetObj != null && targetObj.GetComponent<FreezeObj>() != null)
                    {
                        bool freeze = targetObj.GetComponent<FreezeObj>().freezeActive;

                        targetObj.GetComponent<FreezeObj>().FreezeObject(!freeze);

                        FreezeTarget(false);
                    }
                }
            }
        }
        */
    }

    /*Raycast hits object
     * Bool for whether the object has been detected.
     Bool for obj to check if its frozen
    Once forzen, start timer.

    */

    void FreezeTarget(bool state)
    {
        freezeAim = state;

        float freezeEffect = state ? 0.4f : 0;

        freezableObjects = FindObjectsOfType<FreezeObj>();
        foreach(FreezeObj obj in freezableObjects)
        {
            if (!obj.freezeActive)
            {
                obj.GetComponent<Renderer>().material.SetColor("_EmissionColor", frozenColor);
                obj.GetComponent<Renderer>().material.SetFloat("_FreezeAmount", freezeEffect);
            }
        }
    }
}