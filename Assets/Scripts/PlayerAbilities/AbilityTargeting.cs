using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityTargeting : MonoBehaviour
{
    [Header("Targeted Objects")]
    public GameObject targetObj;
    MobileController mController;

    [Header("Mobile UI Buttons")]
    [SerializeField] Button interactButton;

    public ObjEffect effectableObj;

    public PickUpThrow throwableObj;
    public EventTrigger chargeObj;

    public TriggerSystem triggerObj;

    [Header("Modifiers")]
    public LayerMask effectable;
    public Color highlightedColor;

    [Header("Debugging")]
    public bool targeting;

    void Start()
    {
        Cursor.visible = false;
        mController = GetComponent<MobileController>();

        /*chargeObj = interactButton.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { })*/
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

                if (mController != null)
                {
                    interactButton.gameObject.SetActive(true);

                    interactButton.onClick.RemoveAllListeners();
                    interactButton.onClick.AddListener(throwableObj.Picker);

                    if (throwableObj.holding)
                    {
                        interactButton.onClick.RemoveAllListeners();
                        interactButton.onClick.AddListener(throwableObj.Throw);

                        //chargeObj.OnPointerDown(throwableObj.ChargeObj);
                    }
                }
            }

            else
            {
                if (throwableObj != null)
                {
                    throwableObj.hasplayer = false;
                    throwableObj.pickupComp.ownerID = System.Guid.Empty;
                }

                if (mController != null)
                {
                    interactButton.onClick.RemoveAllListeners();
                    interactButton.gameObject.SetActive(false);
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

                if (mController != null)
                {
                    interactButton.gameObject.SetActive(true);

                    if (triggerObj.triggerType == TriggerSystem.TriggerType.Button)
                        interactButton.onClick.AddListener(triggerObj.ButtonPressed);

                    if (triggerObj.triggerType == TriggerSystem.TriggerType.TimedButton)
                        interactButton.onClick.AddListener(triggerObj.TimedButtonPressed);

                    if (triggerObj.triggerType == TriggerSystem.TriggerType.Lever)
                        interactButton.onClick.AddListener(triggerObj.LeverPulled);
                }
            }

            else
            {
                if (triggerObj != null)
                    triggerObj.hasPlayer = false;

                if (mController != null)
                {
                    interactButton.onClick.RemoveAllListeners();
                    interactButton.gameObject.SetActive(false);
                }

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

                if (mController != null)
                {
                    interactButton.onClick.RemoveAllListeners();
                    interactButton.gameObject.SetActive(false);
                }
            }
        }

        else if (!state)
        {
            targetObj = null;
            effectableObj = null;

        }
    }
}