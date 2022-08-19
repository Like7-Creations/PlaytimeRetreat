using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjResizeAbility : MonoBehaviour
{
    //Objective:
    //Combine the Mass and Scale abilities into a single ability.
    [Header("Vital Components & GameObjects")]
    public AbilityTargeting targeting;
    public GameObject selectedObj;
    public ObjEffect objectEffects;

    [Header("Mass")]
    public float originalMass;
    public float heavyMass;
    public float lightMass;

    [Header("Scale")]
    public Vector3 originalScale;
    public Vector3 shrinkVal;
    public Vector3 growVal;

    void Start()
    {
        targeting = GetComponent<AbilityTargeting>();
    }

    void Update()
    {
        objectEffects = targeting.effectableObj;

        if (Input.GetKeyDown(KeyCode.P))
            if (targeting.targeting)
            {
                if (!objectEffects.shrinkActive)
                {
                    objectEffects.shrinkActive = true;
                    objectEffects.ReturnToNormalSize(true);
                    objectEffects.ModifyObjScalenMass(shrinkVal, lightMass);

                    print("Shrinking Object & Decreasing Mass");
                }
            }

        if (Input.GetKeyDown(KeyCode.O))
            if (targeting.targeting)
            {
                objectEffects.growActive = true;
                objectEffects.ReturnToNormalSize(true);
                objectEffects.ModifyObjScalenMass(growVal, heavyMass);

                print("Gorwing Object & Increasing Mass");
            }
    }
}
