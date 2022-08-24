using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTargeting : MonoBehaviour
{
    [Header("References")]
    PlayerNetComp pcComp;

    [Header("Mobile UI Buttons")]
    [SerializeField] Button interactButton;
    [SerializeField] Button useAbilityButton;
    [SerializeField] Button switchAbilityButton;


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

        pcComp = GetComponent<PlayerNetComp>();
    }

    void Update()
    {
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
                throwableObj.pickupComp.ownerID = pcComp.localID;

                if (pcComp.mobilePlayer)
                    interactButton.gameObject.SetActive(true);
            }

            else
            {
                if (throwableObj != null)
                {
                    throwableObj.hasplayer = false;
                    throwableObj.pickupComp.ownerID = System.Guid.Empty;
                }

                if (pcComp.mobilePlayer)
                    interactButton.gameObject.SetActive(false);

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

                if (pcComp.mobilePlayer)
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

                if (pcComp.mobilePlayer)
                {
                    interactButton.gameObject.SetActive(false);

                    if (triggerObj.triggerType == TriggerSystem.TriggerType.Button)
                        interactButton.onClick.RemoveListener(triggerObj.ButtonPressed);

                    if (triggerObj.triggerType == TriggerSystem.TriggerType.TimedButton)
                        interactButton.onClick.RemoveListener(triggerObj.TimedButtonPressed);

                    if (triggerObj.triggerType == TriggerSystem.TriggerType.Lever)
                        interactButton.onClick.RemoveListener(triggerObj.LeverPulled);
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

                    if (pcComp.mobilePlayer)
                    {
                        switchAbilityButton.gameObject.SetActive(true);
                        useAbilityButton.gameObject.SetActive(true);
                    }
                }

                else
                {
                    targetObj = null;
                    effectableObj = null;

                    if (pcComp.mobilePlayer)
                    {
                        switchAbilityButton.gameObject.SetActive(false);
                        useAbilityButton.gameObject.SetActive(false);
                    }
                }
            }

            else
            {
                targetObj = null;
                effectableObj = null;

                if (pcComp.mobilePlayer)
                {
                    switchAbilityButton.gameObject.SetActive(false);
                    useAbilityButton.gameObject.SetActive(false);
                }
            }
        }

        else if (!state)
        {
            targetObj = null;
            effectableObj = null;

            if (pcComp.mobilePlayer)
            {
                switchAbilityButton.gameObject.SetActive(false);
                useAbilityButton.gameObject.SetActive(false);
            }
        }
    }
}