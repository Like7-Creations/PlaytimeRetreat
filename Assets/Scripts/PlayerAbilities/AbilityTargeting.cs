using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargeting : MonoBehaviour
{
    PlayerNetComp pcComp;
    public GameObject targetObj;

    public ObjEffect effectableObj;
    public PickUpThrow throwableObj;
    public TriggerSystem triggerObj;

    public LayerMask effectable;
    public Color highlightedColor;

    public bool targeting;

    void Start()
    {
        Cursor.visible = false;

        pcComp = GetComponent<PlayerNetComp>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            targeting = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            targeting = false;
        }

        if (targeting)
            Targeting(true);
        else if (!targeting)
            Targeting(false);
    }

    void Targeting(bool state)
    {
        if (state)
        {
            RaycastHit hit;

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, effectable))
            {
                targetObj = hit.collider.gameObject;

                if (targetObj.GetComponent<ObjEffect>())
                {
                    effectableObj = targetObj.GetComponent<ObjEffect>();

                    if (targetObj.GetComponent<PickUpThrow>())
                    {
                        throwableObj = targetObj.GetComponent<PickUpThrow>();
                        throwableObj.hasplayer = true;
                        throwableObj.pickupComp.ownerID = pcComp.localID;
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

                else if (targetObj.GetComponent<TriggerSystem>())
                {
                    triggerObj = targetObj.GetComponent<TriggerSystem>();
                    triggerObj.hasPlayer = true;
                }

                else
                {
                    targetObj = null;
                    effectableObj = null;

                    if (throwableObj != null)
                    {
                        throwableObj.hasplayer = false;
                        throwableObj.pickupComp.ownerID = System.Guid.Empty;
                    }

                    throwableObj = null;

                    if (triggerObj != null)
                        triggerObj.hasPlayer = false;

                    triggerObj = null;
                }
            }

            else
            {
                targetObj = null;
                effectableObj = null;

                if (throwableObj != null)
                {
                    throwableObj.hasplayer = false;
                    throwableObj.pickupComp.ownerID = System.Guid.Empty;
                }

                throwableObj = null;

                if (triggerObj != null)
                    triggerObj.hasPlayer = false;

                triggerObj = null;
            }
        }

        else if (!state)
        {
            targetObj = null;
            effectableObj = null;

            if (throwableObj != null)
            {
                throwableObj.hasplayer = false;
                throwableObj.pickupComp.ownerID = System.Guid.Empty;
            }

            throwableObj = null;

            if (triggerObj != null)
                triggerObj.hasPlayer = false;

            triggerObj = null;
        }
    }
}