using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMassAbility : MonoBehaviour
{
    public AbilityTargeting targeting;
    public GameObject selectedObj;

    public float originalMass;
    public float heavyMass;
    public float lightMass;


    // Start is called before the first frame update
    void Start()
    {
        targeting = GetComponent<AbilityTargeting>();

    }

    // Update is called once per frame
    void Update()
    {
        selectedObj = targeting.targetObj;

        if (Input.GetKeyDown(KeyCode.B))
            if (targeting.targeting)
                if (!selectedObj.GetComponent<ObjEffect>().heavyObj)
                {
                    IncreaseObjMass(selectedObj.GetComponent<Rigidbody>());
                }

        if (Input.GetKeyDown(KeyCode.B))
            if (targeting.targeting)
                if (!selectedObj.GetComponent<ObjEffect>().lightObj)
                {
                    DecreaseObjMass(selectedObj.GetComponent<Rigidbody>());
                }
    }

    public void IncreaseObjMass(Rigidbody rb)
    {
        originalMass = rb.mass;
        rb.mass = heavyMass;
    }

    public void DecreaseObjMass(Rigidbody rb)
    {
        originalMass = rb.mass;
        rb.mass = lightMass;
    }
}
