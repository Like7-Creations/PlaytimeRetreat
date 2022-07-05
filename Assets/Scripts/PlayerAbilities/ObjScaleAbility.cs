using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ObjScaleAbility : Ability
{
    public AbilityTargeting targeting;

    public AbilityHolder abilityHolder;

    public GameObject selectedObj;

    public Vector3 originalScale;
    public Vector3 shrinkVal;
    public Vector3 growVal;

    public override void Activate()
    {
        base.Activate();

        selectedObj = targeting.targetObj;

        if (Input.GetKeyDown(KeyCode.P))
            if (targeting.targeting)
            {
                if (!selectedObj.GetComponent<ObjEffect>().shrinkActive)
                {
                    selectedObj.GetComponent<ObjEffect>().shrinkActive = true;
                    selectedObj.GetComponent<ObjEffect>().ReturnToNormalSize(true);
                    ShrinkObject(selectedObj);
                    Debug.Log("Shrinking Object");
                }
            }

        if (Input.GetKeyDown(KeyCode.O))
            if (targeting.targeting)
            {
                if (!selectedObj.GetComponent<ObjEffect>().growActive)
                {
                    selectedObj.GetComponent<ObjEffect>().growActive = true;
                    selectedObj.GetComponent<ObjEffect>().ReturnToNormalSize(true);
                    GrowObject(selectedObj);
                    Debug.Log("Growing Object");
                }
            }
    }

    void Start()
    {
        targeting = abilityHolder.gameObject.GetComponent<AbilityTargeting>();
    }

    // Update is called once per frame
    /*void Update()
    {
        selectedObj = targeting.targetObj;

        if (Input.GetKeyDown(KeyCode.P))
            if (targeting.targeting)
            {
                if (!selectedObj.GetComponent<ObjEffect>().shrinkActive)
                {
                    selectedObj.GetComponent<ObjEffect>().shrinkActive = true;
                    selectedObj.GetComponent<ObjEffect>().ReturnToNormalSize(true);
                    ShrinkObject(selectedObj);
                    print("Shrinking Object");
                }
            }

        if (Input.GetKeyDown(KeyCode.O))
            if (targeting.targeting)
            {
                if (!selectedObj.GetComponent<ObjEffect>().growActive)
                {
                    selectedObj.GetComponent<ObjEffect>().growActive = true;
                    selectedObj.GetComponent<ObjEffect>().ReturnToNormalSize(true);
                    GrowObject(selectedObj);
                    print("Growing Object");
                }
            }
    }*/

    public void ShrinkObject(GameObject obj)
    {
        originalScale = obj.transform.localScale;
        obj.transform.localScale = shrinkVal;
    }

    public void GrowObject(GameObject obj)
    {
        originalScale = obj.transform.localScale;
        obj.transform.localScale = growVal;
    }
}
