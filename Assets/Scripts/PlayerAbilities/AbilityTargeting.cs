using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargeting : MonoBehaviour
{
    public LayerMask freezeable;

    public bool targeting;

    public GameObject targetObj;
    public ObjEffect[] freezableObjects;

    public Color highlightedColor;

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
            targeting=true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            targeting = false;
        }

        if (targeting)
            Targeting(true);
        else if (!targeting)
            Targeting(false);


        /*
        if (freezeAim)
        {
            

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

    void Targeting(bool state)
    {
        if (state)
        {
            RaycastHit hit;

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, freezeable))
            {
                if (hit.collider.gameObject.GetComponent<ObjEffect>())
                {
                    targetObj = hit.collider.gameObject;

                    //targetObj.GetComponent<Renderer>().material.color = highlightedColor;
                }

                else
                    targetObj = null;
            }

            else
                targetObj = null;
        }

        else if (!state)
        {
            targetObj = null;
        }
    }

    void FreezeTarget(bool state)
    {
        targeting = state;

        float freezeEffect = state ? 0.4f : 0;

        freezableObjects = FindObjectsOfType<ObjEffect>();
        foreach (ObjEffect obj in freezableObjects)
        {
            if (!obj.freezeActive)
            {
                obj.GetComponent<Renderer>().material.SetColor("_EmissionColor", frozenColor);
                obj.GetComponent<Renderer>().material.SetFloat("_FreezeAmount", freezeEffect);
            }
        }
    }
}