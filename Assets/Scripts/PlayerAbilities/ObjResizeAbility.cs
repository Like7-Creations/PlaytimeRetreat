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
                if (!objectEffects.shrinkActive && !objectEffects.growActive)
                {
                    objectEffects.shrinkActive = true;
                    objectEffects.ReturnToNormalSize(true);
                    objectEffects.ModifyObjScalenMass(shrinkVal, ObjEffect.MassType.minMass);

                    print("Shrinking Object & Decreasing Mass");
                }
                else if (objectEffects.growActive && !objectEffects.shrinkActive)
                {
                    objectEffects.growActive = false;
                    objectEffects.ReturnToNormalSize(true);
                    objectEffects.ModifyObjScalenMass(originalScale, ObjEffect.MassType.defaultMass);

                    print("Shrinking Object & Decreasing Mass");
                }
            }

        if (Input.GetKeyDown(KeyCode.O))
            if (targeting.targeting)
            {
                if (!objectEffects.growActive && !objectEffects.shrinkActive)
                {
                    objectEffects.growActive = true;
                    objectEffects.ReturnToNormalSize(true);
                    objectEffects.ModifyObjScalenMass(growVal, ObjEffect.MassType.maxMass);

                    print("Gorwing Object & Increasing Mass");
                }
                else if (!objectEffects.growActive && objectEffects.shrinkActive)
                {
                    objectEffects.shrinkActive = false;
                    objectEffects.ReturnToNormalSize(true);
                    objectEffects.ModifyObjScalenMass(originalScale, ObjEffect.MassType.defaultMass);

                    print("Shrinking Object & Decreasing Mass");
                }
            }
    }
}
