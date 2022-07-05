using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSurfaceAbility : MonoBehaviour
{
    public AbilityTargeting targeting;

    public GameObject selectedObj;
    //public Collider selectedCollider;

    void Start()
    {
        targeting = GetComponent<AbilityTargeting>();
    }

    void Update()
    {
        selectedObj = targeting.targetObj;
        //selectedCollider = targeting.targetObj.GetComponent<Collider>();

        if (Input.GetKeyDown(KeyCode.B))
            if (targeting.targeting)
                if (!selectedObj.GetComponent<ObjEffect>().bounceActive)
                {
                    selectedObj.GetComponent<ObjEffect>().bounceActive = true;
                    selectedObj.GetComponent<ObjEffect>().DisableBounce(true);
                    EnableBounce(selectedObj.GetComponent<Collider>());
                    print("Activate Bounce");
                }

        if(Input.GetKeyDown(KeyCode.N))
            if(targeting.targeting)
                if (!selectedObj.GetComponent<ObjEffect>().frictionInactive)
                {
                    selectedObj.GetComponent<ObjEffect>().frictionInactive = true;
                    selectedObj.GetComponent<ObjEffect>().EnableFriction(true);
                    DisableFriction(selectedObj.GetComponent<Collider>());
                    print("Deactivate Friction");
                }
    }

    public void EnableBounce(Collider col)
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
    }
}