using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTargeting : MonoBehaviour
{

    [Header("Targeted Objects")]
    public GameObject targetObj;

    public ObjEffect effectableObj;
    public PickUpThrow throwableObj;
    public TriggerSystem triggerObj;

    [Header("Modifiers")]
    public LayerMask effectable;
    public Color highlightedColor;

    [Header("Debugging")]
    public bool targeting;

    void Start()
    {
        Cursor.visible = false;
        //pcComp = GetComponent<PlayerNetComp>();
    }

    void Update()
    {
        if (triggerObj != null && triggerObj.hasPlayer)
        {
            triggerObj.hasPlayer = false;
            triggerObj = null;
        }
        if (throwableObj != null && throwableObj.hasplayer)
        {
            throwableObj.hasplayer = false;
            throwableObj = null;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        TargetingPickUp(ray);
        TargetingTrigger(ray);

        if (Input.GetMouseButtonDown(1))
        {
            targeting = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            targeting = false;
        }

        if (targeting)
            TargetingObj(ray, true);
        else if (!targeting)
            TargetingObj(ray, false);
    }

    void TargetingPickUp(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, effectable))
        {
            targetObj = hit.collider.gameObject;

            if (targetObj.GetComponent<PickUpThrow>())
            {
                throwableObj = targetObj.GetComponent<PickUpThrow>();
                throwableObj.hasplayer = true;
                //throwableObj.pickupComp.ownerID = pcComp.localID;
            }

            else
            {
                if (throwableObj != null)
                {
                    throwableObj.hasplayer = false;
                    throwableObj.pickupComp.ownerID = System.Guid.Empty;
                }

                throwableObj = null;
            }
        }
    }

    void TargetingTrigger(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, effectable))
        {
            targetObj = hit.collider.gameObject;

            if (targetObj.GetComponent<TriggerSystem>())
            {
                triggerObj = targetObj.GetComponent<TriggerSystem>();
                triggerObj.hasPlayer = true;
            }

            else
            {
                if (triggerObj != null)
                    triggerObj.hasPlayer = false;

                triggerObj = null;
            }
        }
    }

    void TargetingObj(Ray ray, bool state)
    {
        if (state)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, effectable))
            {
                targetObj = hit.collider.gameObject;

                if (targetObj.GetComponent<ObjEffect>())
                {
                    effectableObj = targetObj.GetComponent<ObjEffect>();

                }

                else
                {
                    targetObj = null;
                    effectableObj = null;

                }
            }

            else
            {
                targetObj = null;
                effectableObj = null;

            }
        }

        else if (!state)
        {
            targetObj = null;
            effectableObj = null;

        }
    }
}