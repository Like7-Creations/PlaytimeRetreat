using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSurfaceAbility : MonoBehaviour
{
    [Header("Vital Components & GameObjects")]
    public AbilityTargeting targeting;
    public GameObject selectedObj;
    public ObjEffect objectEffects;

    //public Collider selectedCollider;

    void Start()
    {
        targeting = GetComponent<AbilityTargeting>();
    }

    void Update()
    {
        //selectedCollider = targeting.targetObj.GetComponent<Collider>();
        objectEffects = targeting.effectableObj;

        if (Input.GetKeyDown(KeyCode.B))
            if (targeting.targeting)
                if (!objectEffects.bounceActive)
                {
                    objectEffects.bounceActive = true;

                    objectEffects.DisableBounce(true);
                    objectEffects.EnableBounce(selectedObj.GetComponent<Collider>());

                    print("Activate Bounce");
                }

        if(Input.GetKeyDown(KeyCode.N))
            if(targeting.targeting)
                if (!objectEffects.frictionInactive)
                {
                    objectEffects.frictionInactive = true;

                    objectEffects.EnableFriction(true);
                    objectEffects.DisableFriction(selectedObj.GetComponent<Collider>());

                    print("Deactivate Friction");
                }
    }

    /*public void EnableBounce(Collider col)
    {
        col = selectedObj.GetComponent<Collider>();

        col.material.bounciness = 1;
        col.material.bounceCombine = PhysicMaterialCombine.Maximum;
        print("Bouncyness Activate");
    }

    public void DisableFriction(Collider col)
    {
        col = selectedObj.GetComponent<Collider>();

        col.material.dynamicFriction = 0.2f;
        col.attachedRigidbody.mass += 20;

        col.material.frictionCombine = PhysicMaterialCombine.Minimum;

        print("Object is Slippery Now");
    }*/
}